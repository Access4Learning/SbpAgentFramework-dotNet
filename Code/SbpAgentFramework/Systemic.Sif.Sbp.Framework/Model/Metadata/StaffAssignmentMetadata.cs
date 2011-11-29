/*
* Copyright 2011 Systemic Pty Ltd
* 
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*    http://www.apache.org/licenses/LICENSE-2.0
* 
* Unless required by applicable law or agreed to in writing, software distributed under the License 
* is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
* or implied.
* See the License for the specific language governing permissions and limitations under the License.
*/

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
