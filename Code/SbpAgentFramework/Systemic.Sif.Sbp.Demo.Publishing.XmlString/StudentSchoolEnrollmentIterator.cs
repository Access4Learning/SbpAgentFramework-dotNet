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
using Systemic.Sif.Framework.Model;

namespace Systemic.Sif.Sbp.Demo.Publishing.XmlString
{

    class StudentSchoolEnrollmentIterator : GenericIterator<StudentSchoolEnrollment>
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static int eventMessageCount = 0;
        private static int responseMessageCount = 0;

        bool onceOnly = true;
        private SifParser sifParser = SifParser.NewInstance();
        private string[] messages = new string[]
        {
            @"
              <StudentSchoolEnrollment RefId=""A8C3D3E34B359D75101D00AA001A1652"">
                <StudentPersonalRefId>7C834EA9EDA12090347F83297E1C290C</StudentPersonalRefId>
                <SchoolInfoRefId>D3E34B359D75101A8C3D00AA001A1652</SchoolInfoRefId>
                <MembershipType>01</MembershipType>
                <TimeFrame>C</TimeFrame>
                <SchoolYear>2004</SchoolYear>
                <EntryDate>2004-01-29</EntryDate>
                <EntryType>
                <Code>1838</Code>
                </EntryType>
                <YearLevel>
                <Code>10</Code>
                </YearLevel>
                <FTE>1.00</FTE>
                <FTPTStatus>01</FTPTStatus>
              </StudentSchoolEnrollment>
            "
        };

        public override SifEvent<StudentSchoolEnrollment> GetNextEvent()
        {
            StudentSchoolEnrollment studentSchoolEnrollment = (StudentSchoolEnrollment)sifParser.Parse(messages[eventMessageCount]);
            eventMessageCount++;
            if (log.IsDebugEnabled) log.Debug("StudentSchoolEnrollmentIterator data " + studentSchoolEnrollment.ToXml() + ".");
            SifEvent<StudentSchoolEnrollment> sifEvent = new SifEvent<StudentSchoolEnrollment>(studentSchoolEnrollment, EventAction.Change);
            return sifEvent;
        }

        public override bool HasNextEvent()
        {
            bool hasNext = (eventMessageCount < messages.Length);

            if (!onceOnly && !hasNext)
            {
                eventMessageCount = 0;
            }

            return hasNext;
        }

        public override StudentSchoolEnrollment GetNextResponse()
        {
            StudentSchoolEnrollment studentSchoolEnrollment = (StudentSchoolEnrollment)sifParser.Parse(messages[responseMessageCount++]);
            responseMessageCount++;
            return studentSchoolEnrollment;
        }

        public override bool HasNextResponse()
        {
            bool hasNext = (responseMessageCount < messages.Length);

            if (!onceOnly && !hasNext)
            {
                responseMessageCount = 0;
            }

            return hasNext;
        }

    }

}
