using Edustructures.SifWorks.School;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Subscriber.Baseline
{

    public abstract class SchoolInfoSubscriber : WithoutDependentsCachingSubscriber<SchoolInfo>
    {

        public SchoolInfoSubscriber()
            : base()
        {
        }

        public SchoolInfoSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
