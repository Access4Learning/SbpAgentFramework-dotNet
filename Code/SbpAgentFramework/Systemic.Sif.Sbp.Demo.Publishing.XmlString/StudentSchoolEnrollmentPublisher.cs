using Edustructures.SifWorks;
using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Framework.Publisher;

namespace Systemic.Sif.Sbp.Demo.Publishing.XmlString
{

    class StudentSchoolEnrollmentPublisher : Systemic.Sif.Sbp.Framework.Publisher.Baseline.StudentSchoolEnrollmentPublisher
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AgentProperties agentProperties;

        public override int EventFrequency
        {
            get { return agentProperties.GetProperty("publisher." + SifObjectType.Name + ".eventFrequency", 3600000); }
            set { }
        }

        public StudentSchoolEnrollmentPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
            agentProperties = new AgentProperties(null);
            AgentConfiguration.GetAgentProperties(agentProperties);
        }

        public override ISifEventIterator<StudentSchoolEnrollment> GetSifEvents()
        {
            return new StudentSchoolEnrollmentIterator();
        }

        public override ISifResponseIterator<StudentSchoolEnrollment> GetSifResponses(Query query, IZone zone)
        {
            return new StudentSchoolEnrollmentIterator();
        }

    }

}
