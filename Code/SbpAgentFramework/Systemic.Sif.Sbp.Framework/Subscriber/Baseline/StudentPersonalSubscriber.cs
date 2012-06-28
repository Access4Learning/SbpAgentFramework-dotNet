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

using OpenADK.Library.au.Student;
using OpenADK.Library.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Subscriber.Baseline
{

    /// <summary>
    /// A Subscriber of StudentPersonal.
    /// </summary>
    public abstract class StudentPersonalSubscriber : WithoutDependentsCachingSubscriber<StudentPersonal>
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StudentPersonalSubscriber()
            : base()
        {
        }

        /// <summary>
        /// This constructor specifies the configuration settings associated with this Subscriber.
        /// </summary>
        /// <param name="agentConfig">Configuration settings associated with this Subscriber.</param>
        public StudentPersonalSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
