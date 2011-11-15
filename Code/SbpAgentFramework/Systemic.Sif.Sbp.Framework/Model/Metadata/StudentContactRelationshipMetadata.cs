using System.Collections.Generic;
using System.Collections.ObjectModel;
using Edustructures.SifWorks.Student;

namespace Systemic.Sif.Sbp.Framework.Model.Metadata
{

    class StudentContactRelationshipMetadata : SifDataObjectMetadata<StudentContactRelationship>
    {

        public override ICollection<DependentObject> DependentObjects
        {

            get
            {

                return new Collection<DependentObject>()
                {
                    new DependentObject() { SifObjectName = "StudentPersonal", ObjectKeyValue = "@RefId=" + sifDataObject.StudentPersonalRefId },
                    new DependentObject() { SifObjectName = "StudentContactPersonal", ObjectKeyValue = "@RefId=" + sifDataObject.StudentContactPersonalRefId }
                };

            }

        }

        public override string SifUniqueId
        {
            get { return "@StudentPersonalRefId=" + sifDataObject.StudentPersonalRefId + "|@StudentContactPersonalRefId=" + sifDataObject.StudentContactPersonalRefId; }
        }

        public StudentContactRelationshipMetadata(StudentContactRelationship studentContactRelationship)
            : base(studentContactRelationship)
        {
        }

    }

}
