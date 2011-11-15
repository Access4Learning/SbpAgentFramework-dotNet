using Edustructures.SifWorks;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Framework.Subscriber;
using Systemic.Sif.Sbp.Framework.Service;

namespace Systemic.Sif.Sbp.Framework.Subscriber
{

    /// <summary>
    /// This class implements the Sync Control Functionality through configuration specified in database tables rather
    /// than through the default behaviour of the SIF Common Framework.
    /// It is important to note that if the SIF Common Framework property file turns off sync altogether, then the
    /// method in this class has no affect (meaning it will be ignored).
    /// </summary>
    /// <typeparam name="T">The type of SIF Data Object this Subscriber processes, e.g. StudentPersonal, Schoolnfo, etc.</typeparam>
    public abstract class SyncSubscriber<T> : GenericSubscriber<T> where T : SifDataObject, new()
    {
        int requestFrequency = 0;

        protected override int RequestFrequency
        {
            get { return requestFrequency; }
            set { requestFrequency = value; }
        }

        public SyncSubscriber()
            : base()
        {
        }

        public SyncSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

        protected override bool MakeRequest(IZone zone)
        {
            return false;
        }

        /// <summary>
        /// Override the default behavious of the SIF Common Framework base Subscriber.
        /// </summary>
        /// <param name="zone">Zone to synchronise with.</param>
        /// <see cref="Systemic.Sif.Framework.Subscriber.GenericSubscriber{T}.Sync(Edustructures.SifWorks.IZone)"/>
        public override void Sync(IZone zone)
        {
            ISyncService syncService = new SyncService();

            if (syncService.IsSyncRequired(SifObjectType.Name, AgentConfiguration.SourceId, zone.ZoneId))
            {
                base.Sync(zone);
                syncService.MarkAsSynced(SifObjectType.Name, AgentConfiguration.SourceId, zone.ZoneId);
            }

        }

    }

}
