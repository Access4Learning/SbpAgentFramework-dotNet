using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Publisher.Baseline
{

    public abstract class StudentPersonalPublisher : GenericPublisher<StudentPersonal>
    {

        public StudentPersonalPublisher()
            : base()
        {
        }

        public StudentPersonalPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
