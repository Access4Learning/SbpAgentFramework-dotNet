using Edustructures.SifWorks.Learning;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Sbp.Framework.Model.Metadata;

namespace Systemic.Sif.Sbp.Framework.Subscriber.Baseline
{

    public abstract class TeachingGroupSubscriber : WithDependentsCachingSubscriber<TeachingGroup>
    {

        public TeachingGroupSubscriber()
            : base()
        {
        }

        public TeachingGroupSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

        internal override SifDataObjectMetadata<TeachingGroup> MetadataInstance(TeachingGroup teachingGroup)
        {
            return new TeachingGroupMetadata(teachingGroup);
        }

    }

}
