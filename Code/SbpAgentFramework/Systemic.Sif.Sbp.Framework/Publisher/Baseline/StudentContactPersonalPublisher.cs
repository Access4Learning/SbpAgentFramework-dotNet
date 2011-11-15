using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Publisher.Baseline
{

    public abstract class StudentContactPersonalPublisher : GenericPublisher<StudentContactPersonal>
    {

        public StudentContactPersonalPublisher()
            : base()
        {
        }

        public StudentContactPersonalPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
