using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Subscriber.Baseline
{

    public abstract class StudentPersonalSubscriber : WithoutDependentsCachingSubscriber<StudentPersonal>
    {

        public StudentPersonalSubscriber()
            : base()
        {
        }

        public StudentPersonalSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
