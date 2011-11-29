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

using System;
using Edustructures.SifWorks;
using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Framework.Publisher;

namespace Systemic.Sif.Sbp.Demo.Publishing.ResponseManager
{

    class StudentPersonalPublisher : Systemic.Sif.Sbp.Framework.Publisher.Baseline.StudentPersonalPublisher
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AgentProperties agentProperties;

        public override int EventFrequency
        {
            get { return agentProperties.GetProperty("publisher." + SifObjectType.Name + ".eventFrequency", 0); }
            set { }
        }

        private string ExtractPrimaryKey(Query query)
        {
            string key = null;

            // Unable to manage complex queries (containing more than just primary keys).
            if (query.Conditions.Length == 1)
            {
                ConditionGroup conditions = query.Conditions[0];

                // StudentPersonal has only a single primary key.
                if (conditions.Size() == 1)
                {
                    Condition condition = conditions.HasCondition("@RefId");

                    if ((ComparisonOperators.EQ.Equals(condition.Operators)))
                    {
                        key = condition.Value;
                    }

                }

            }

            return key;
        }

        public StudentPersonalPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
            agentProperties = new AgentProperties(null);
            AgentConfiguration.GetAgentProperties(agentProperties);
        }

        public override ISifEventIterator<StudentPersonal> GetSifEvents()
        {
            return new StudentPersonalIterator(null);
        }

        public override ISifResponseIterator<StudentPersonal> GetSifResponses(Query query, IZone zone)
        {
            StudentPersonalIterator studentPersonalIterator = null;

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (query.HasConditions)
            {
                string key = ExtractPrimaryKey(query);

                if (key == null)
                {
                    throw new SifException(SifErrorCategoryCode.RequestResponse, SifErrorCodes.REQRSP_UNSUPPORTED_QUERY_9, "SIF Query not supported.", zone);
                }
                else
                {
                    if (log.IsDebugEnabled) log.Debug("SIF Response requested for StudentPersonal with a SIF RefId of " + key + ".");
                    studentPersonalIterator = new StudentPersonalIterator(key);
                }

            }
            else
            {
                if (log.IsDebugEnabled) log.Debug("SIF Response requested for all StudentPersonals.");
                studentPersonalIterator = new StudentPersonalIterator(null);
            }

            return studentPersonalIterator;
        }

    }

}
