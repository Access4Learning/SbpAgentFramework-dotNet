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

using OpenADK.Library;
using OpenADK.Library.au.Student;
using OpenADK.Library.Tools.Cfg;
using Systemic.Sif.Framework.Publisher;

namespace Systemic.Sif.Sbp.Demo.Publishing.XmlString
{

    class StudentSchoolEnrollmentPublisher : Systemic.Sif.Sbp.Framework.Publisher.Baseline.StudentSchoolEnrollmentPublisher
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AgentProperties agentProperties;

        public override int EventFrequency
        {
            get { return agentProperties.GetProperty("publisher." + SifObjectType.Name + ".eventFrequency", 3600000); }
            set { }
        }

        public StudentSchoolEnrollmentPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
            agentProperties = new AgentProperties(null);
            AgentConfiguration.GetAgentProperties(agentProperties);
        }

        public override ISifEventIterator<StudentSchoolEnrollment> GetSifEvents()
        {
            return new StudentSchoolEnrollmentIterator();
        }

        public override ISifResponseIterator<StudentSchoolEnrollment> GetSifResponses(Query query, IZone zone)
        {
            return new StudentSchoolEnrollmentIterator();
        }

    }

}
