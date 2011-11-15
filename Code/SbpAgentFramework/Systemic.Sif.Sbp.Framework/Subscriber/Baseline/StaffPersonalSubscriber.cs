using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Subscriber.Baseline
{

    public abstract class StaffPersonalSubscriber : WithoutDependentsCachingSubscriber<StaffPersonal>
    {

        public StaffPersonalSubscriber()
            : base()
        {
        }

        public StaffPersonalSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
