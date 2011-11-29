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

using Edustructures.SifWorks;
using Edustructures.SifWorks.Student;
using Systemic.Sif.Framework.Model;

namespace Systemic.Sif.Sbp.Demo.Publishing.XmlString
{

    class StudentPersonalIterator : GenericIterator<StudentPersonal>
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
              <StudentPersonal RefId=""7C834EA9EDA12090347F83297E1C290C"">
                <LocalId>S1234567</LocalId>
                <PersonInfo>
                  <Name Type=""LGL"">
                    <FamilyName>Smith</FamilyName>
                    <GivenName>Fred</GivenName>
                    <FullName>Fred Smith</FullName>
                  </Name>
                </PersonInfo>
              </StudentPersonal>
            "
        };

        public override SifEvent<StudentPersonal> GetNextEvent()
        {
            StudentPersonal studentPersonal = (StudentPersonal)sifParser.Parse(messages[eventMessageCount]);
            eventMessageCount++;
            if (log.IsDebugEnabled) log.Debug("StudentPersonalIterator data " + studentPersonal.ToXml() + ".");
            SifEvent<StudentPersonal> sifEvent = new SifEvent<StudentPersonal>(studentPersonal, EventAction.Change);
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

        public override StudentPersonal GetNextResponse()
        {
            StudentPersonal studentPersonal = (StudentPersonal)sifParser.Parse(messages[responseMessageCount++]);
            responseMessageCount++;
            return studentPersonal;
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
