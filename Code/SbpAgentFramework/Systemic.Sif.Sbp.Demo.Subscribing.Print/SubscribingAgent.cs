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

        public override IList<IBaseSubscriber> GetSubscribers()
        {
            IList<IBaseSubscriber> subscribers = new List<IBaseSubscriber>();
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
