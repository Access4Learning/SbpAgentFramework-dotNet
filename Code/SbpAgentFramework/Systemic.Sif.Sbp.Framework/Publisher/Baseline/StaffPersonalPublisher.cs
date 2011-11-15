using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Publisher.Baseline
{

    public abstract class StaffPersonalPublisher : GenericPublisher<StaffPersonal>
    {

        public StaffPersonalPublisher()
            : base()
        {
        }

        public StaffPersonalPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
