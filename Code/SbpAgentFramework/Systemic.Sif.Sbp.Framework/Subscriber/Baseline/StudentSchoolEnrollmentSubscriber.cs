using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Sbp.Framework.Model.Metadata;

namespace Systemic.Sif.Sbp.Framework.Subscriber.Baseline
{

    public abstract class StudentSchoolEnrollmentSubscriber : WithDependentsCachingSubscriber<StudentSchoolEnrollment>
    {

        public StudentSchoolEnrollmentSubscriber()
            : base()
        {
        }

        public StudentSchoolEnrollmentSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

        internal override SifDataObjectMetadata<StudentSchoolEnrollment> MetadataInstance(StudentSchoolEnrollment studentSchoolEnrollment)
        {
            return new StudentSchoolEnrollmentMetadata(studentSchoolEnrollment);
        }

    }

}
