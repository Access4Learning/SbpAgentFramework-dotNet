
namespace Systemic.Sif.Sbp.Framework.Agent
{

    public abstract class PublishingAgent : Systemic.Sif.Framework.Agent.PublishingAgent
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This constructor will create a publishing Agent using the default "agent.cfg" file. If this configuration
        /// file does not exist, the Agent will not run when called.
        /// </summary>
        public PublishingAgent()
            : base()
        {
        }

        /// <summary>
        /// This constructor defines the configuration file associated with this Agent.
        /// </summary>
        /// <param name="cfgFileName">Configuration file associated with this Agent. If not provided, assumes "agent.cfg".</param>
        public PublishingAgent(string cfgFileName)
            : base(cfgFileName)
        {
        }

    }

}
