using System;
using System.Collections.Generic;
using System.Timers;
using Edustructures.SifWorks;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Framework.Model;
using Systemic.Sif.Sbp.Framework.Model;
using Systemic.Sif.Sbp.Framework.Model.Metadata;
using Systemic.Sif.Sbp.Framework.Service;

namespace Systemic.Sif.Sbp.Framework.Subscriber
{

    /// <summary>
    /// This class implements some of the functionality defined in the SIF Baseline Profile (SBP). They are all
    /// infrastructure level functions and include: start-up Sync Control through configuration defined in a database,
    /// and a Dependent Object Cache (DOC).
    /// Subscribers of SIF Data Objects that are defined as part of the (SBP) should extend this class.
    /// It will not be possible for a Subscriber of SIF Data Objects that are NOT defined the the SBP to extend this
    /// class. The NonCachingSubscriber should be used in this case.
    /// </summary>
    /// <typeparam name="T">The type of SIF Data Object this Subscriber processes (other than those defined in the SBP).</typeparam>
    public abstract class WithoutDependentsCachingSubscriber<T> : CachingSubscriber<T> where T : SifDataObject, new()
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IDependentObjectCacheService cacheService;

        public WithoutDependentsCachingSubscriber()
            : base()
        {
            cacheService = new DependentObjectCacheService();
        }

        public WithoutDependentsCachingSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
            cacheService = new DependentObjectCacheService();
        }

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
                if (log.IsDebugEnabled) log.Debug(metadata.ObjectName + " (" + metadata.SifUniqueId + ") removed from the cache as it has now been received.");
                cacheService.DeleteDependentObject(dependentObject);
            }
            else
            {
                if (log.IsDebugEnabled) log.Debug(metadata.ObjectName + " (" + metadata.SifUniqueId + ") did not exist in the cache.");
            }

            return true;
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
                processFurther = PreProcessSifDataObject(sifEvent.SifDataObject, zone);
            }

            return processFurther;
        }

        protected override bool PreProcessResponse(T sifDataObject, IZone zone)
        {
            return PreProcessSifDataObject(sifDataObject, zone);
        }

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

        public override void StartRequestProcessing(IZone[] zones)
        {
            base.StartRequestProcessing(zones);

            // Create a timer with an appropriate interval.
            if (CacheCheckFrequency > 0)
            {
                if (log.IsDebugEnabled) log.Debug(this.GetType().Name + " started requesting dependent objects (interval is " + CacheCheckFrequency + " milliseconds)...");
                Timer timer = new Timer(CacheCheckFrequency);
                timer.Elapsed += delegate { RequestDependentObjects(zones); };
                timer.Start();
            }

        }

    }

}
