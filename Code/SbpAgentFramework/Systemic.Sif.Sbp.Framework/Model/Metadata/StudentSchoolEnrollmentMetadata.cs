using System.Collections.Generic;
using System.Collections.ObjectModel;
using Edustructures.SifWorks.Student;

namespace Systemic.Sif.Sbp.Framework.Model.Metadata
{

    class StudentSchoolEnrollmentMetadata : SifDataObjectMetadata<StudentSchoolEnrollment>
    {

        public override ICollection<DependentObject> DependentObjects
        {

            get
            {

                return new Collection<DependentObject>()
                {
                    new DependentObject() { SifObjectName = "StudentPersonal", ObjectKeyValue = "@RefId=" + sifDataObject.StudentPersonalRefId },
                    new DependentObject() { SifObjectName = "SchoolInfo", ObjectKeyValue = "@RefId=" + sifDataObject.SchoolInfoRefId }
                };

            }

        }

        public StudentSchoolEnrollmentMetadata(StudentSchoolEnrollment studentSchoolEnrollment)
            : base(studentSchoolEnrollment)
        {
        }

    }

}
