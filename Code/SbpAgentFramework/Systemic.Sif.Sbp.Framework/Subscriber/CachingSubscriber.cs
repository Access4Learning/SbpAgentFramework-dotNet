using Edustructures.SifWorks;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Sbp.Framework.Model.Metadata;

namespace Systemic.Sif.Sbp.Framework.Subscriber
{

    public abstract class CachingSubscriber<T> : SyncSubscriber<T> where T : SifDataObject, new()
    {
        protected abstract int CacheCheckFrequency { get; set; }

        public CachingSubscriber()
            : base()
        {
        }

        public CachingSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

        internal virtual SifDataObjectMetadata<T> MetadataInstance(T sifDataObject)
        {
            return new SifDataObjectMetadata<T>(sifDataObject);
        }

    }

}
