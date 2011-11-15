using System.Collections.Generic;
using System.Collections.ObjectModel;
using Edustructures.SifWorks.Student;

namespace Systemic.Sif.Sbp.Framework.Model.Metadata
{

    class StaffAssignmentMetadata : SifDataObjectMetadata<StaffAssignment>
    {
 
        public override ICollection<DependentObject> DependentObjects
        {

            get
            {

                return new Collection<DependentObject>()
                {
                    new DependentObject() { SifObjectName = "SchoolInfo", ObjectKeyValue = "@RefId=" + sifDataObject.SchoolInfoRefId },
                    new DependentObject() { SifObjectName = "StaffPersonal", ObjectKeyValue = "@RefId=" + sifDataObject.StaffPersonalRefId }
                };

            }

        }

        public StaffAssignmentMetadata(StaffAssignment staffAssignment)
            : base(staffAssignment)
        {
        }

   }

}
