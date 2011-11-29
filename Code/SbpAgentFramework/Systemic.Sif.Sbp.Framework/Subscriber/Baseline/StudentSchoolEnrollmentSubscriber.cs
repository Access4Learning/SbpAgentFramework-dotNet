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
