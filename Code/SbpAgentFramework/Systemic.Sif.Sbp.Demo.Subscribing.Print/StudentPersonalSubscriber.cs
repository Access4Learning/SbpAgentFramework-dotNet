using Edustructures.SifWorks;
using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Framework.Model;

namespace Systemic.Sif.Sbp.Demo.Subscribing.Print
{

    class StudentPersonalSubscriber : Systemic.Sif.Sbp.Framework.Subscriber.Baseline.StudentPersonalSubscriber
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AgentProperties agentProperties;

        protected override int CacheCheckFrequency
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.checkFrequency", 3600000); }
            set { }
        }

        public StudentPersonalSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
            agentProperties = new AgentProperties(null);
            AgentConfiguration.GetAgentProperties(agentProperties);
        }

        protected override void ProcessEvent(SifEvent<StudentPersonal> sifEvent, IZone zone)
        {
            if (log.IsDebugEnabled) log.Debug(sifEvent.SifDataObject.ToXml());
            if (log.IsDebugEnabled) log.Debug("Received a " + sifEvent.EventAction.ToString() + " event for StudentPersonal in Zone " + zone.ZoneId + ".");
        }

        protected override void ProcessResponse(StudentPersonal sifDataObject, IZone zone)
        {
            if (log.IsDebugEnabled) log.Debug(sifDataObject.ToXml());
            if (log.IsDebugEnabled) log.Debug("Received a request response for StudentPersonal in Zone " + zone.ZoneId + ".");
        }

    }

}
