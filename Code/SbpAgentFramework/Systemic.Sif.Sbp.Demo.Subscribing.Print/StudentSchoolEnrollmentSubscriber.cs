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
using Systemic.Sif.Framework.Model;

namespace Systemic.Sif.Sbp.Demo.Subscribing.Print
{

    class StudentSchoolEnrollmentSubscriber : Systemic.Sif.Sbp.Framework.Subscriber.Baseline.StudentSchoolEnrollmentSubscriber
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AgentProperties agentProperties;

        protected override int CacheCheckFrequency
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.checkFrequency", 3600000); }
            set { }
        }

        protected override int ExpiryPeriod
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.expiryPeriod", 7200000); }
            set { }
        }

        protected override string ExpiryStrategy
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.expiryStrategy", "REQUEST"); }
            set { }
        }

        public StudentSchoolEnrollmentSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
            agentProperties = new AgentProperties(null);
            AgentConfiguration.GetAgentProperties(agentProperties);
        }

        protected override void ProcessEvent(SifEvent<StudentSchoolEnrollment> sifEvent, IZone zone)
        {
            if (log.IsDebugEnabled) log.Debug(sifEvent.SifDataObject.ToXml());
            if (log.IsDebugEnabled) log.Debug("Received a " + sifEvent.EventAction.ToString() + " event for StudentSchoolEnrollment in Zone " + zone.ZoneId + ".");
        }

        protected override void ProcessResponse(StudentSchoolEnrollment sifDataObject, IZone zone)
        {
            if (log.IsDebugEnabled) log.Debug(sifDataObject.ToXml());
            if (log.IsDebugEnabled) log.Debug("Received a request response for StudentSchoolEnrollment in Zone " + zone.ZoneId + ".");
        }

        protected override bool DoesObjectExistInTargetSystem(string dependentObjectName, string objectKeyValue)
        {
            bool exists = false;

            if ("SchoolInfo".Equals(dependentObjectName) && "@RefId=D3E34B359D75101A8C3D00AA001A1652".Equals(objectKeyValue))
            {
                exists = false;
            }
            else if ("StudentPersonal".Equals(dependentObjectName) && "@RefId=7C834EA9EDA12090347F83297E1C290C".Equals(objectKeyValue))
            {
                exists = false;
            }

            return exists;
        }

    }

}
