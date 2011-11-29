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
using Edustructures.SifWorks.School;
using Systemic.Sif.Framework.Model;

namespace Systemic.Sif.Sbp.Demo.Publishing.XmlString
{

    class SchoolInfoIterator : GenericIterator<SchoolInfo>
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
              <SchoolInfo RefId=""D3E34B359D75101A8C3D00AA001A1652"">
                <LocalId>01011234</LocalId>
                <StateProvinceId>01011234</StateProvinceId>
                <CommonwealthId>012345</CommonwealthId>
                <SchoolName>Lincoln Secondary College</SchoolName>
                <SchoolDistrict> Southern Metropolitan Region</SchoolDistrict>
                <SchoolType>Pri/Sec</SchoolType>
                <SchoolSector>NG</SchoolSector>
                <IndependentSchool>Y</IndependentSchool>
                <NonGovSystemicStatus>S</NonGovSystemicStatus>
                <System>0003</System>
                <ReligiousAffiliation>2171</ReligiousAffiliation>
                <LocalGovernmentArea>Cardinia</LocalGovernmentArea>
                <SLA>205801452</SLA>
              </SchoolInfo>
            "
        };

        public override SifEvent<SchoolInfo> GetNextEvent()
        {
            SchoolInfo schoolInfo = (SchoolInfo)sifParser.Parse(messages[eventMessageCount]);
            eventMessageCount++;
            if (log.IsDebugEnabled) log.Debug("SchoolInfoIterator data " + schoolInfo.ToXml() + ".");
            SifEvent<SchoolInfo> sifEvent = new SifEvent<SchoolInfo>(schoolInfo, EventAction.Change);
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

        public override SchoolInfo GetNextResponse()
        {
            SchoolInfo schoolInfo = (SchoolInfo)sifParser.Parse(messages[responseMessageCount++]);
            responseMessageCount++;
            return schoolInfo;
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
