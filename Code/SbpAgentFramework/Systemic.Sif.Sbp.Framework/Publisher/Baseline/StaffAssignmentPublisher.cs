using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Publisher.Baseline
{

    public abstract class StaffAssignmentPublisher : GenericPublisher<StaffAssignment>
    {

        public StaffAssignmentPublisher()
            : base()
        {
        }

        public StaffAssignmentPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
