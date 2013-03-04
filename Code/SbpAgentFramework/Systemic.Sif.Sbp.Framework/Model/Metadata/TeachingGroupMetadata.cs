/*
* Copyright 2011-2013 Systemic Pty Ltd
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
using OpenADK.Library.au.Learning;

namespace Systemic.Sif.Sbp.Framework.Model.Metadata
{

    public class TeachingGroupMetadata : SifDataObjectMetadata<TeachingGroup>
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
