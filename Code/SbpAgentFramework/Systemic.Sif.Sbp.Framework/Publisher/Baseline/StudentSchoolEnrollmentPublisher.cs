using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Publisher.Baseline
{

    public abstract class StudentSchoolEnrollmentPublisher : GenericPublisher<StudentSchoolEnrollment>
    {

        public StudentSchoolEnrollmentPublisher()
            : base()
        {
        }

        public StudentSchoolEnrollmentPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
