/*
* Copyright 2012 Systemic Pty Ltd
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
using System.Collections.Generic;
using Systemic.Sif.Framework.Subscriber;

namespace Systemic.Sif.Sbp.Demo.Subscribing.Print
{

    class SubscribingAgent : Systemic.Sif.Sbp.Framework.Agent.SubscribingAgent
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Simply call the default constructor of the super class.
        /// </summary>
        public SubscribingAgent() : base() 
        {
        }

        /// <summary>
        /// Define the configuration file used for creating this instance.
        /// </summary>
        /// <param name="cfgFileName">Configuration file associated with this SIF Agent.</param>
        public SubscribingAgent(String cfgFileName)
            : base(cfgFileName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Return the Subscribers instantiated for this Agent.
        /// </summary>
        /// <returns>Subscribers instantiated for this Agent.</returns>
        public override IList<IBaseSubscriber> GetSubscribers()
        {
            IList<IBaseSubscriber> subscribers = new List<IBaseSubscriber>();
            IdentitySubscriber identitySubscriber = new IdentitySubscriber(AgentConfiguration);
            identitySubscriber.ApplicationId = Properties.GetProperty("agent.applicationId", "defaultId");
            subscribers.Add(identitySubscriber);
            StudentPersonalSubscriber studentPersonalSubscriber = new StudentPersonalSubscriber(AgentConfiguration);
            studentPersonalSubscriber.ApplicationId = Properties.GetProperty("agent.applicationId", "defaultId");
            subscribers.Add(studentPersonalSubscriber);
            SchoolInfoSubscriber schoolInfoSubscriber = new SchoolInfoSubscriber(AgentConfiguration);
            schoolInfoSubscriber.ApplicationId = Properties.GetProperty("agent.applicationId", "defaultId");
            subscribers.Add(schoolInfoSubscriber);
            StudentSchoolEnrollmentSubscriber studentSchoolEnrollmentSubscriber = new StudentSchoolEnrollmentSubscriber(AgentConfiguration);
            studentSchoolEnrollmentSubscriber.ApplicationId = Properties.GetProperty("agent.applicationId", "defaultId");
            subscribers.Add(studentSchoolEnrollmentSubscriber);
            return subscribers;
        }

        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            SubscribingAgent agent = null;

            // Check if the appropriate number of parameters has been passed.
            if (args.Length == 0)
            {
                agent = new SubscribingAgent("SubscribingAgent.cfg");
            }
            else if (args.Length == 1)
            {
                agent = new SubscribingAgent(args[0]);
            }
            else
            {
                Console.WriteLine("Usage is: " + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " <agent_config_file>");
                Environment.Exit(1);
            }

            try
            {
                agent.Run();
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled) log.Error("Unable to run the SIF Agent", e);
            }

        }

    }

}
