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
    /// Subscribers of SIF Data Objects which do not have dependent objects (e.g. StudentPersonal, SchoolInfo) and are
    /// defined as part of the SBP should extend this class.
    /// Subscribers of SIF Data Objects that are not defined as part of the SBP should extend the NonCachingSubscriber.
    /// </summary>
    /// <typeparam name="T">The type of SIF Data Object this Subscriber processes.</typeparam>
    public abstract class WithoutDependentsCachingSubscriber<T> : CachingSubscriber<T> where T : SifDataObject, new()
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IDependentObjectCacheService cacheService = new DependentObjectCacheService();

        /// <summary>
        /// Create an instance of the Subscriber without referencing the Agent configuration settings.
        /// </summary>
        public WithoutDependentsCachingSubscriber()
            : base()
        {
        }

        /// <summary>
        /// Create an instance of the Subscriber based upon the Agent configuration settings.
        /// </summary>
        /// <param name="agentConfig">Agent configuration settings.</param>
        /// <exception cref="System.ArgumentException">agentConfig parameter is null.</exception>
        public WithoutDependentsCachingSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

        /// <summary>
        /// This method will check whether the specified sifDataObject exists as a dependent object in the cache. If
        /// it does, it is no longer required and can be removed.
        /// </summary>
        /// <param name="sifDataObject">SIF Data Object to check against the cache.</param>
        /// <param name="zone">Zone the SIF Data Object was received from.</param>
        /// <returns>This method will always return true.</returns>
        /// <exception cref="System.ArgumentException">sifDataObject or zone parameter is null.</exception>
        private bool PreProcessSifDataObject(T sifDataObject, IZone zone)
        {

            if (sifDataObject == null)
            {
                throw new ArgumentNullException("sifDataObject");
            }

            if (zone == null)
            {
                throw new ArgumentNullException("zone");
            }

            SifDataObjectMetadata<T> metadata = MetadataInstance(sifDataObject);
            if (log.IsDebugEnabled) log.Debug(this.GetType().Name + " preprocessing " + metadata.ObjectName + " (" + metadata.SifUniqueId + ") for application " + ApplicationId + " in zone " + zone.ZoneId + ".");
            DependentObject dependentObject = cacheService.RetrieveDependentObject(metadata.ObjectName, metadata.SifUniqueId, ApplicationId, zone.ZoneId);

            // If this object exists as a dependent object in the cache, it is now no longer required and can be
            // removed.
            if (dependentObject != null)
            {
                if (log.IsInfoEnabled) log.Info(metadata.ObjectName + " (" + metadata.SifUniqueId + ") removed from the cache as it has now been received.");
                cacheService.DeleteDependentObject(dependentObject);
            }
            else
            {
                if (log.IsDebugEnabled) log.Debug(metadata.ObjectName + " (" + metadata.SifUniqueId + ") did not exist in the cache.");
            }

            return true;
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
                    processFurther = PreProcessSifDataObject(sifEvent.SifDataObject, zone);
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
                processFurther = PreProcessSifDataObject(sifDataObject, zone);
            }

            return processFurther;
        }

        /// <summary>
        /// This method will check the cache for dependent objects that have yet to be received and make a SIF
        /// Request them.
        /// </summary>
        /// <param name="zones">Zones that the SIF Request need to be made on.</param>
        private void RequestDependentObjects(IZone[] zones)
        {

            foreach (IZone zone in zones)
            {
                ICollection<DependentObject> dependentObjects = cacheService.RetrieveNotYetRequested(SifObjectType.Name, ApplicationId, zone.ZoneId);
                if (log.IsDebugEnabled) log.Debug(this.GetType().Name + " found " + dependentObjects.Count + " " + SifObjectType.Name + " dependent objects for application " + ApplicationId + " in zone " + zone.ZoneId + ".");

                foreach (DependentObject dependentObject in dependentObjects)
                {
                    // Create a Query for the SIF Data Object type.
                    Query query = new Query(SifObjectType);
                    // Without this, an error occurs.
                    query.SifVersions = new SifVersion[] { AgentConfiguration.Version };
                    ICollection<SifRefIdMetadata> metadataValues = SifDataObjectMetadata<T>.ParseSifUniqueId(dependentObject.ObjectKeyValue);

                    foreach (SifRefIdMetadata metadataValue in metadataValues)
                    {
                        query.AddCondition(metadataValue.XPath, ComparisonOperators.EQ, metadataValue.Value);
                    }

                    zone.Query(query);
                    cacheService.MarkAsRequested(dependentObject, AgentConfiguration.SourceId, zone.ZoneId);
                    if (log.IsDebugEnabled) log.Debug("Made a request for " + SifObjectType.Name + " with SIF RefId credentials of " + dependentObject.ObjectKeyValue + ".");
                }

            }

        }

        /// <summary>
        /// This method will start a thread for requesting missing dependent objects.
        /// </summary>
        /// <param name="zones">Zones that the request need to be made on.</param>
        public override void StartRequestProcessing(IZone[] zones)
        {
            base.StartRequestProcessing(zones);
            if (log.IsDebugEnabled) log.Debug("Caching has been " + (CacheEnabled ? "enabled" : "disabled") + " for Subscriber " + this.GetType().Name + ".");

            // Create a timer with an appropriate interval.
            if (CacheEnabled && CacheCheckFrequency > 0)
            {
                if (log.IsDebugEnabled) log.Debug(this.GetType().Name + " started requesting dependent objects (interval is " + CacheCheckFrequency + " milliseconds)...");
                Timer timer = new Timer(CacheCheckFrequency);
                timer.Elapsed += delegate { RequestDependentObjects(zones); };
                timer.Start();
            }

        }

    }

}
