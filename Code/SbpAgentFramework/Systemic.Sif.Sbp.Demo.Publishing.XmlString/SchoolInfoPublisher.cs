using Edustructures.SifWorks;
using Edustructures.SifWorks.School;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Framework.Publisher;

namespace Systemic.Sif.Sbp.Demo.Publishing.XmlString
{

    class SchoolInfoPublisher : Systemic.Sif.Sbp.Framework.Publisher.Baseline.SchoolInfoPublisher
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AgentProperties agentProperties;

        public override int EventFrequency
        {
            get { return agentProperties.GetProperty("publisher." + SifObjectType.Name + ".eventFrequency", 3600000); }
            set { }
        }

        public SchoolInfoPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
            agentProperties = new AgentProperties(null);
            AgentConfiguration.GetAgentProperties(agentProperties);
        }

        public override ISifEventIterator<SchoolInfo> GetSifEvents()
        {
            return new SchoolInfoIterator();
        }

        public override ISifResponseIterator<SchoolInfo> GetSifResponses(Query query, IZone zone)
        {
            return new SchoolInfoIterator();
        }

    }

}
