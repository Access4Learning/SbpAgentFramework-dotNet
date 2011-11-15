using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Publisher.Baseline
{

    public abstract class StudentContactRelationshipPublisher : GenericPublisher<StudentContactRelationship>
    {

        public StudentContactRelationshipPublisher()
            : base()
        {
        }

        public StudentContactRelationshipPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
