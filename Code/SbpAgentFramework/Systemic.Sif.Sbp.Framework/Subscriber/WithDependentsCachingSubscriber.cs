/*
* Copyright 2011-2013 Systemic Pty Ltd
* 
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*    http://www.apache.org/licenses/LICENSE-2.0
* 
* Unless required by applicable law or agreed to in writing, software distributed under the License 
* is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
* or implied.
* See the License for the specific language governing permissions and limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using OpenADK.Library;
using OpenADK.Library.Tools.Cfg;
using Systemic.Sif.Framework.Model;
using Systemic.Sif.Sbp.Framework.Model;
using Systemic.Sif.Sbp.Framework.Model.Metadata;
using Systemic.Sif.Sbp.Framework.Service;

namespace Systemic.Sif.Sbp.Framework.Subscriber
{

    /// <summary>
    /// This class implements some of the functionality defined in the SIF Baseline Profile (SBP). This includes
    /// infrastructure level functions such as management of synchronisation requests and the Dependent Object Cache
    /// (DOC).
    /// Subscribers of SIF Data Objects which have dependent objects (e.g. Identity, TeachingGroup) and are defined as
    /// part of the SBP should extend this class.
    /// Subscribers of SIF Data Objects that are not defined as part of the SBP should extend the NonCachingSubscriber.
    /// </summary>
    /// <typeparam name="T">The type of SIF Data Object this Subscriber processes.</typeparam>
    public abstract class WithDependentsCachingSubscriber<T> : CachingSubscriber<T> where T : SifDataObject, new()
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AgentProperties agentProperties = new AgentProperties(null);
        private IDependentObjectCacheService cacheService = new DependentObjectCacheService();

        protected virtual int ExpiryPeriod
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.expiryPeriod", 7200000); }
            set { }
        }

        protected virtual string ExpiryStrategy
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.expiryStrategy", "REQUEST"); }
            set { }
        }

        /// <summary>
        /// Create an instance of the Subscriber without referencing the Agent configuration settings.
        /// </summary>
        public WithDependentsCachingSubscriber()
            : base()
        {
        }

        /// <summary>
        /// Create an instance of the Subscriber based upon the Agent configuration settings.
        /// </summary>
        /// <param name="agentConfig">Agent configuration settings.</param>
        /// <exception cref="System.ArgumentException">agentConfig parameter is null.</exception>
        public WithDependentsCachingSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
            agentConfig.GetAgentProperties(agentProperties);
            if (log.IsDebugEnabled) log.Debug("Subscriber " + this.GetType().Name + " has a cache expiry period of " + ExpiryPeriod + " and expiry strategy of " + ExpiryStrategy + ".");
        }

        /// <summary>
        /// Use this method to check if the dependent object (defined by the dependentObjectName and keyValues) for
        /// the SIF Data Object (sifDataObject) exists in the target system. If it exists, then TRUE must be returned;
        /// otherwise FALSE must be returned.
        /// 
        /// If this method returns TRUE, then the SIF Data Object will be cached and its dependent objects will
        /// automatically be requested by an appropriate Subscriber (which can be a Subscriber from another Agent).
        /// 
        /// The keyValues hold the primary key (name as XPath and value) of the dependent object. In most cases this is
        /// a SIF RefId, but in some cases can be something else. The keyValues parameter is a list of key values (in
        /// case the primary key is made up of a compound key). The list will be in the correct order as defined by the
        /// SIF Specification.
        /// 
        /// dependentObjectName and keyValues can be extracted from the SIF Data Object itself, but is provided as a
        /// parameter for convenience. There might be the need to get other information out of the SIF Data Object to
        /// implement this method successfully.
        /// 
        /// Example of parameters: StudentSchoolEnrollment
        /// 
        /// <code>
        /// <StudentSchoolEnrollment RefId="A8C3D3E34B359D75101D00AA001A1652">
        ///   <StudentPersonalRefId>D3E34B359D75101A8C3D00AA001A1652</StudentPersonalRefId>
        ///   <SchoolInfoRefId>A4E33E359D99101A8C3D00AA001BB76E</SchoolInfoRefId>
        ///   <MembershipType>01</MembershipType>
        ///    .....
        /// </StudentSchoolEnrollment>
        /// </code>
        /// 
        /// For the above example SIF Data Object, the sifDataObject parameter would be StudentSchoolEnrollment and
        /// this method would be called twice because there are 2 dependent objects:
        /// - The Student with RefId=D3E34B359D75101A8C3D00AA001A1652
        /// - The School with RefId=A4E33E359D99101A8C3D00AA001BB76E
        /// 
        /// The dependentObjectName and keyValues parameters for the two calls would be:
        /// 1st Call (Student): sifObjectName=StudentPersonal
        ///                     keyValues[0].xpathToKey=@RefId, keyValues[0].keyValue=D3E34B359D75101A8C3D00AA001A1652
        /// 2nd Call (School) : sifObjectName=SchoolInfo
        ///                     keyValues[0].xpathToKey=@RefId, keyValues[0].keyValue=A4E33E359D99101A8C3D00AA001BB76E
        /// </summary>
        /// <param name="dependentObjectName">Name of the dependent object.</param>
        /// <param name="keyValues">Key(s) associated with the dependent object.</param>
        /// <param name="sifDataObject">SIF Data Object associated with the dependent object.</param>
        /// <returns>True if the dependent object exists; false otherwise.</returns>
        protected abstract bool DoesObjectExistInTargetSystem(string dependentObjectName, string objectKeyValue);

        /// <summary>
        /// This method will check the specified sifDataObject to see whether its dependent objects already exist in
        /// the target system or in the cache. If all the dependent objects exist in the target system, then the
        /// sifDataObject can be processed further (return true). If some dependent objects are have been cached, then
        /// the sifDataObject will be cached awaiting all outstanding dependent objects, and this method will return
        /// false.
        /// </summary>
        /// <param name="sifDataObject">SIF Data Object to check against the cache.</param>
        /// <param name="eventAction">The action associated with the SIF Data Object, i.e. add, change.</param>
        /// <param name="zone">Zone the SIF Data Object was received from.</param>
        /// <returns>True if all dependent objects exist in the target system; false otherwise.</returns>
        /// <exception cref="System.ArgumentException">sifDataObject or zone parameter is null.</exception>
        private bool PreProcessSifDataObject(T sifDataObject, EventAction? eventAction, IZone zone)
        {

            if (sifDataObject == null)
            {
                throw new ArgumentNullException("sifDataObject");
            }

            if (zone == null)
            {
                throw new ArgumentNullException("zone");
            }

            bool processFurther = true;
            SifDataObjectMetadata<T> metadata = MetadataInstance(sifDataObject);
            if (log.IsDebugEnabled) log.Debug(this.GetType().Name + " preprocessing " + metadata.ObjectName + " (" + metadata.SifUniqueId + ") for application " + ApplicationId + " in zone " + zone.ZoneId + ".");
            CachedObject cachedObject = cacheService.RetrieveCachedObject(metadata.ObjectName, metadata.SifUniqueId, ApplicationId, zone.ZoneId);

            // Previously cached SIF Data Objects/messages are currently ignored.
            // TODO: Implement a better solution that manages previously received messages.
            if (cachedObject == null)
            {
                if (log.IsDebugEnabled) log.Debug(metadata.ObjectName + " (" + metadata.SifUniqueId + ") does not exist in the cache and it's dependents will be checked.");
                ICollection<DependentObject> dependentObjects = metadata.DependentObjects;
                ICollection<DependentObject> existingObjects = new Collection<DependentObject>();

                foreach (DependentObject dependentObject in dependentObjects)
                {

                    // The dependent object does not exist in the cache.
                    if (cacheService.RetrieveDependentObject(dependentObject.SifObjectName, dependentObject.ObjectKeyValue, ApplicationId, zone.ZoneId) == null)
                    {
                        if (log.IsDebugEnabled) log.Debug("Dependent " + dependentObject.SifObjectName + " (" + dependentObject.ObjectKeyValue + ") did NOT exist in the cache.");

                        // The dependent objects exists in the target system so there is no need to cache it.
                        if (DoesObjectExistInTargetSystem(dependentObject.SifObjectName, dependentObject.ObjectKeyValue))
                        {
                            if (log.IsDebugEnabled) log.Debug("Dependent " + dependentObject.SifObjectName + " (" + dependentObject.ObjectKeyValue + ") did exist in the target system and will NOT be cached.");
                            existingObjects.Add(dependentObject);
                        }
                        else
                        {
                            if (log.IsDebugEnabled) log.Debug("Dependent " + dependentObject.SifObjectName + " (" + dependentObject.ObjectKeyValue + ") did NOT exist in the target system and will be cached.");
                        }

                    }
                    // The dependent object exists in the cache so there is no need to cache it again.
                    else
                    {
                        if (log.IsDebugEnabled) log.Debug("Dependent " + dependentObject.SifObjectName + " (" + dependentObject.ObjectKeyValue + ") did exist in the cache and will NOT be cached.");
                        existingObjects.Add(dependentObject);
                    }

                }

                foreach (DependentObject existingObject in existingObjects)
                {
                    dependentObjects.Remove(existingObject);
                }

                // There are outstanding dependent objects.
                if (dependentObjects.Count != 0)
                {
                    if (log.IsDebugEnabled) log.Debug(metadata.ObjectName + " (" + metadata.SifUniqueId + ") will be cached and as not all it's dependents exist in the target system.");
                    processFurther = false;
                    cacheService.StoreObjectInCache
                        (metadata.ObjectName,
                        metadata.SifUniqueId,
                        sifDataObject.ToXml(),
                        (eventAction == null ? null : eventAction.ToString()),
                        AgentConfiguration.SourceId,
                        ApplicationId,
                        zone.ZoneId,
                        ExpiryStrategy,
                        ExpiryPeriod,
                        dependentObjects);
                }
                else
                {
                    if (log.IsDebugEnabled) log.Debug(metadata.ObjectName + " (" + metadata.SifUniqueId + ") will NOT be cached as all it's dependents exist in the target system.");
                }

            }
            else
            {
                processFurther = false;
                if (log.IsDebugEnabled) log.Debug(metadata.ObjectName + " (" + metadata.SifUniqueId + ") already exists in the cache and will be ignored.");
            }

            return processFurther;
        }

        /// <summary>
        /// This method will perform pre-processing of the received event.
        /// </summary>
        /// <param name="sifEvent">SIF Event received.</param>
        /// <param name="zone">Zone that the SIF Event was received from.</param>
        /// <returns>True if the event needs to be processed further; false otherwise.</returns>
        protected override bool PreProcessEvent(SifEvent<T> sifEvent, IZone zone)
        {

            if (sifEvent == null)
            {
                throw new ArgumentNullException("sifEvent");
            }

            bool processFurther = true;

            if (CacheEnabled)
            {

                // The cache does not manage Undefined or Delete events.
                if (EventAction.Add.Equals(sifEvent.EventAction) || EventAction.Change.Equals(sifEvent.EventAction))
                {
                    processFurther = PreProcessSifDataObject(sifEvent.SifDataObject, sifEvent.EventAction, zone);
                }

            }

            return processFurther;
        }

        /// <summary>
        /// This method will perform pre-processing of the received SIF Data Object.
        /// </summary>
        /// <param name="sifDataObject">SIF Data Object received.</param>
        /// <param name="zone">Zone that the SIF Data Object was received from.</param>
        /// <returns>True if the SIF Data Object needs to be processed further; false otherwise.</returns>
        protected override bool PreProcessResponse(T sifDataObject, IZone zone)
        {
            bool processFurther = true;

            if (CacheEnabled)
            {
                processFurther = PreProcessSifDataObject(sifDataObject, null, zone);
            }

            return processFurther;
        }

        /// <summary>
        /// This method will process all cached objects that have expired (based upon expiry period and strategy).
        /// </summary>
        private void ProcessExpiredObjects()
        {
            cacheService.UpdateExpiredObjects(ApplicationId, AgentConfiguration.SourceId, ExpiryStrategy, ExpiryPeriod);
        }

        /// <summary>
        /// This method will check the cache for cached objects whose dependent objects have been received, and
        /// process them accordingly.
        /// </summary>
        /// <param name="zones">Zones that the cached objects will be processed against.</param>
        private void ProcessObjectsWithoutDependents(IZone[] zones)
        {
            ICollection<CachedObject> cachedObjects = cacheService.RetrieveByNoDependencies(SifObjectType.Name, ApplicationId, AgentConfiguration.SourceId);
            if (log.IsDebugEnabled) log.Debug(this.GetType().Name + " found " + cachedObjects.Count + " objects with no dependencies for application " + ApplicationId + " and Agent " + AgentConfiguration.SourceId + ".");
            IDictionary<string, IZone> zonesMap = new Dictionary<string, IZone>();

            // Create an indexed list of Zones for easy access.
            foreach (IZone zone in zones)
            {
                zonesMap.Add(zone.ZoneId, zone);
            }

            foreach (CachedObject cachedObject in cachedObjects)
            {
                T sifDataObject = SifDataObjectMetadata<T>.CreateFromXml(cachedObject.ObjectXml);
                IZone zone;

                if (cachedObject.IsEvent)
                {
                    EventAction eventAction = (EventAction)Enum.Parse(typeof(EventAction), cachedObject.EventType, true);
                    SifEvent<T> sifEvent = new SifEvent<T>(sifDataObject, eventAction);

                    if (zonesMap.TryGetValue(cachedObject.ZoneId, out zone))
                    {
                        ProcessEvent(sifEvent, zone);
                    }

                }
                else
                {

                    if (zonesMap.TryGetValue(cachedObject.ZoneId, out zone))
                    {
                        ProcessResponse(sifDataObject, zone);
                    }

                }

                cacheService.DeleteCachedObject(cachedObject);
                if (log.IsDebugEnabled) log.Debug("Processed " + cachedObject.SifObjectName + " (" + cachedObject.ObjectKeyValue + ") and removed from cache.");
            }

        }

        /// <summary>
        /// This method will start a thread for processing cached objects with no dependencies and cached objects that
        /// have expired.
        /// </summary>
        /// <param name="zones">Zones that the cached objects will be processed against.</param>
        public override void StartRequestProcessing(IZone[] zones)
        {
            base.StartRequestProcessing(zones);
            if (log.IsDebugEnabled) log.Debug("Caching has been " + (CacheEnabled ? "enabled" : "disabled") + " for Subscriber " + this.GetType().Name + ".");

            // Create a timer with an appropriate interval.
            if (CacheEnabled && CacheCheckFrequency > 0)
            {
                if (log.IsDebugEnabled) log.Debug(this.GetType().Name + " started processing of expired objects and objects without dependents (interval is " + CacheCheckFrequency + " milliseconds)...");
                Timer timer = new Timer(CacheCheckFrequency);
                timer.Elapsed += delegate { ProcessObjectsWithoutDependents(zones); ProcessExpiredObjects(); };
                timer.Start();
            }

        }

    }

}
