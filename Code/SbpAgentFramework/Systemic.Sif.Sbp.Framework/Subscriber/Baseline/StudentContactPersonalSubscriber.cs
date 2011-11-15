using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Subscriber.Baseline
{

    public abstract class StudentContactPersonalSubscriber : WithoutDependentsCachingSubscriber<StudentContactPersonal>
    {

        public StudentContactPersonalSubscriber()
            : base()
        {
        }

        public StudentContactPersonalSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
