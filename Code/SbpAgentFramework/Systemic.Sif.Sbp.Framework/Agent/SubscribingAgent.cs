
namespace Systemic.Sif.Sbp.Framework.Agent
{

    public abstract class SubscribingAgent : Systemic.Sif.Framework.Agent.SubscribingAgent
    {

        /// <summary>
        /// This constructor will create a subscribing Agent using the default "agent.cfg" file. If this configuration
        /// file does not exist, the Agent will not run when called.
        /// </summary>
        public SubscribingAgent()
            : base()
        {
        }

        /// <summary>
        /// This constructor defines the configuration file associated with this Agent.
        /// </summary>
        /// <param name="cfgFileName">Configuration file associated with this Agent. If not provided, assumes "agent.cfg".</param>
        public SubscribingAgent(string cfgFileName)
            : base(cfgFileName)
        {
        }

    }

}
