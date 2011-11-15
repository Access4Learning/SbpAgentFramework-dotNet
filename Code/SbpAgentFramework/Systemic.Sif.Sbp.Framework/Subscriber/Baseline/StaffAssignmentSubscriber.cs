using Edustructures.SifWorks.Student;
using Systemic.Sif.Sbp.Framework.Model.Metadata;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Subscriber.Baseline
{

    public abstract class StaffAssignmentSubscriber : WithDependentsCachingSubscriber<StaffAssignment>
    {

        public StaffAssignmentSubscriber()
            : base()
        {
        }

        public StaffAssignmentSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

        internal override SifDataObjectMetadata<StaffAssignment> MetadataInstance(StaffAssignment staffAssignment)
        {
            return new StaffAssignmentMetadata(staffAssignment);
        }

    }

}
