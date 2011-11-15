using Edustructures.SifWorks.Student;
using Systemic.Sif.Sbp.Framework.Model.Metadata;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Subscriber.Baseline
{

    public abstract class StudentContactRelationshipSubscriber : WithDependentsCachingSubscriber<StudentContactRelationship>
    {

        public StudentContactRelationshipSubscriber()
            : base()
        {
        }

        public StudentContactRelationshipSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

        internal override SifDataObjectMetadata<StudentContactRelationship> MetadataInstance(StudentContactRelationship studentContactRelationship)
        {
            return new StudentContactRelationshipMetadata(studentContactRelationship);
        }

    }

}
