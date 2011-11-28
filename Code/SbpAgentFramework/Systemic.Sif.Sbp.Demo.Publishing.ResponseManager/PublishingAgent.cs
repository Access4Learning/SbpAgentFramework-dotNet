using System;
using System.Collections.Generic;
using Systemic.Sif.Framework.Publisher;

namespace Systemic.Sif.Sbp.Demo.Publishing.ResponseManager
{

    class PublishingAgent : Systemic.Sif.Sbp.Framework.Agent.PublishingAgent
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Simply call the default constructor of the super class.
        /// </summary>
        public PublishingAgent()
            : base()
        {
        }

        /// <summary>
        /// Define the configuration file used for creating this instance.
        /// </summary>
        /// <param name="cfgFileName">Configuration file associated with this SIF Agent.</param>
        public PublishingAgent(String cfgFileName)
            : base(cfgFileName)
        {
        }

        public override IList<IBasePublisher> GetPublishers()
        {
            IList<IBasePublisher> publishers = new List<IBasePublisher>();
            publishers.Add(new StudentPersonalPublisher(AgentConfiguration));
            return publishers;
        }

        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            PublishingAgent agent = null;

            // Check if the appropriate number of parameters has been passed.
            if (args.Length == 0)
            {
                agent = new PublishingAgent("PublishingAgent.cfg");
            }
            else if (args.Length == 1)
            {
                agent = new PublishingAgent(args[0]);
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
