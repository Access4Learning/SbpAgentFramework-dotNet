using System.Collections.Generic;
using System.Collections.ObjectModel;
using Edustructures.SifWorks.Learning;

namespace Systemic.Sif.Sbp.Framework.Model.Metadata
{

    class TeachingGroupMetadata : SifDataObjectMetadata<TeachingGroup>
    {

        public override ICollection<DependentObject> DependentObjects
        {

            get
            {

                ICollection<DependentObject> dependentObjects = new Collection<DependentObject>()
                {
                    new DependentObject() { SifObjectName = "SchoolInfo", ObjectKeyValue = "@RefId=" + sifDataObject.SchoolInfoRefId }
                };

                foreach (TeachingGroupStudent teachingGroupStudent in sifDataObject.StudentList)
                {
                    DependentObject dependentObject = new DependentObject() { SifObjectName = "StudentPersonal", ObjectKeyValue = "@RefId=" + teachingGroupStudent.StudentPersonalRefId };
                    dependentObjects.Add(dependentObject);
                }

                foreach (TeachingGroupTeacher teachingGroupTeacher in sifDataObject.TeacherList)
                {
                    DependentObject dependentObject = new DependentObject() { SifObjectName = "StaffPersonal", ObjectKeyValue = "@RefId=" + teachingGroupTeacher.StaffPersonalRefId };
                    dependentObjects.Add(dependentObject);
                }

                return dependentObjects;
            }

        }

        public TeachingGroupMetadata(TeachingGroup teachingGroup)
            : base(teachingGroup)
        {
        }

    }

}
