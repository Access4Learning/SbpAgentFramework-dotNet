using Edustructures.SifWorks;
using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Framework.Model;

namespace Systemic.Sif.Sbp.Demo.Subscribing.Print
{

    class StudentSchoolEnrollmentSubscriber : Systemic.Sif.Sbp.Framework.Subscriber.Baseline.StudentSchoolEnrollmentSubscriber
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AgentProperties agentProperties;

        protected override int CacheCheckFrequency
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.checkFrequency", 3600000); }
            set { }
        }

        protected override int ExpiryPeriod
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.expiryPeriod", 7200000); }
            set { }
        }

        protected override string ExpiryStrategy
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.expiryStrategy", "REQUEST"); }
            set { }
        }

        public StudentSchoolEnrollmentSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
            agentProperties = new AgentProperties(null);
            AgentConfiguration.GetAgentProperties(agentProperties);
        }

        protected override void ProcessEvent(SifEvent<StudentSchoolEnrollment> sifEvent, IZone zone)
        {
            if (log.IsDebugEnabled) log.Debug(sifEvent.SifDataObject.ToXml());
            if (log.IsDebugEnabled) log.Debug("Received a " + sifEvent.EventAction.ToString() + " event for StudentSchoolEnrollment in Zone " + zone.ZoneId + ".");
        }

        protected override void ProcessResponse(StudentSchoolEnrollment sifDataObject, IZone zone)
        {
            if (log.IsDebugEnabled) log.Debug(sifDataObject.ToXml());
            if (log.IsDebugEnabled) log.Debug("Received a request response for StudentSchoolEnrollment in Zone " + zone.ZoneId + ".");
        }

        protected override bool DoesObjectExistInTargetSystem(string dependentObjectName, string objectKeyValue)
        {
            bool exists = false;

            if ("SchoolInfo".Equals(dependentObjectName) && "@RefId=D3E34B359D75101A8C3D00AA001A1652".Equals(objectKeyValue))
            {
                exists = false;
            }
            else if ("StudentPersonal".Equals(dependentObjectName) && "@RefId=7C834EA9EDA12090347F83297E1C290C".Equals(objectKeyValue))
            {
                exists = false;
            }

            return exists;
        }

    }

}
