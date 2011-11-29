/*
* Copyright 2011 Systemic Pty Ltd
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
using Edustructures.SifWorks;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Framework.Model;
using Systemic.Sif.Sbp.Framework.Model;
using Systemic.Sif.Sbp.Framework.Model.Metadata;
using Systemic.Sif.Sbp.Framework.Service;

namespace Systemic.Sif.Sbp.Framework.Subscriber
{

    public abstract class WithDependentsCachingSubscriber<T> : CachingSubscriber<T> where T : SifDataObject, new()
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DependentObjectCacheService cacheService = new DependentObjectCacheService();
        int expiryPeriod = 7200000;
        string expiryStrategy = "REQUEST";

        protected virtual int ExpiryPeriod
        {
            get { return expiryPeriod; }
            set { expiryPeriod = value; }
        }

        protected virtual string ExpiryStrategy
        {
            get { return expiryStrategy; }
            set { expiryStrategy = value; }
        }

        public WithDependentsCachingSubscriber()
            : base()
        {
        }

        public WithDependentsCachingSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
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
                        eventAction.ToString(),
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

        protected override bool PreProcessEvent(SifEvent<T> sifEvent, IZone zone)
        {

            if (sifEvent == null)
            {
                throw new ArgumentNullException("sifEvent");
            }

            bool processFurther = true;

            // The cache does not manage Undefined or Delete events.
            if (EventAction.Add.Equals(sifEvent.EventAction) || EventAction.Change.Equals(sifEvent.EventAction))
            {
                processFurther = PreProcessSifDataObject(sifEvent.SifDataObject, sifEvent.EventAction, zone);
            }

            return processFurther;
        }

        protected override bool PreProcessResponse(T sifDataObject, IZone zone)
        {
            return PreProcessSifDataObject(sifDataObject, null, zone);
        }

        private void ProcessExpiredObjects()
        {
            cacheService.UpdateExpiredObjects(ApplicationId, AgentConfiguration.SourceId, ExpiryStrategy, ExpiryPeriod);
        }

        private void ProcessObjectsWithoutDependents(IZone[] zones)
        {
            ICollection<CachedObject> cachedObjects = cacheService.RetrieveByNoDependencies(SifObjectType.Name, ApplicationId, AgentConfiguration.SourceId);
            if (log.IsDebugEnabled) log.Debug(this.GetType().Name + " found " + cachedObjects.Count + " objects with no dependencies for application " + ApplicationId + " and Agent " + AgentConfiguration.SourceId + ".");
            IDictionary<string, IZone> zonesMap = new Dictionary<string, IZone>();

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

        public override void StartRequestProcessing(IZone[] zones)
        {
            base.StartRequestProcessing(zones);

            // Create a timer with an appropriate interval.
            if (CacheCheckFrequency > 0)
            {
                if (log.IsDebugEnabled) log.Debug(this.GetType().Name + " started processing of expired objects and objects without dependents (interval is " + CacheCheckFrequency + " milliseconds)...");
                Timer timer = new Timer(CacheCheckFrequency);
                timer.Elapsed += delegate { ProcessObjectsWithoutDependents(zones); ProcessExpiredObjects(); };
                timer.Start();
            }

        }

    }

}
