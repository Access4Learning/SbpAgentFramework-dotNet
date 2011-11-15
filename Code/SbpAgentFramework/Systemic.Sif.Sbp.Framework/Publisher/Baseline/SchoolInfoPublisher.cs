using Edustructures.SifWorks.School;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Publisher.Baseline
{

    public abstract class SchoolInfoPublisher : GenericPublisher<SchoolInfo>
    {

        public SchoolInfoPublisher()
            : base()
        {
        }

        public SchoolInfoPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
