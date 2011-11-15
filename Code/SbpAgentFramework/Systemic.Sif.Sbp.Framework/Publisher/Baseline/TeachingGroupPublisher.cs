using Edustructures.SifWorks.Learning;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Publisher.Baseline
{

    public abstract class TeachingGroupPublisher : GenericPublisher<TeachingGroup>
    {

        public TeachingGroupPublisher()
            : base()
        {
        }

        public TeachingGroupPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
