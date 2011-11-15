using Edustructures.SifWorks;
using Edustructures.SifWorks.Student;
using Systemic.Sif.Framework.Model;

namespace Systemic.Sif.Sbp.Demo.Publishing.XmlString
{

    class StudentPersonalIterator : GenericIterator<StudentPersonal>
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static int eventMessageCount = 0;
        private static int responseMessageCount = 0;

        bool onceOnly = true;
        private SifParser sifParser = SifParser.NewInstance();
        private string[] messages = new string[]
        {
            @"
              <StudentPersonal RefId=""7C834EA9EDA12090347F83297E1C290C"">
                <LocalId>S1234567</LocalId>
                <PersonInfo>
                  <Name Type=""LGL"">
                    <FamilyName>Smith</FamilyName>
                    <GivenName>Fred</GivenName>
                    <FullName>Fred Smith</FullName>
                  </Name>
                </PersonInfo>
              </StudentPersonal>
            "
        };

        public override SifEvent<StudentPersonal> GetNextEvent()
        {
            StudentPersonal studentPersonal = (StudentPersonal)sifParser.Parse(messages[eventMessageCount]);
            eventMessageCount++;
            if (log.IsDebugEnabled) log.Debug("StudentPersonalIterator data " + studentPersonal.ToXml() + ".");
            SifEvent<StudentPersonal> sifEvent = new SifEvent<StudentPersonal>(studentPersonal, EventAction.Change);
            return sifEvent;
        }

        public override bool HasNextEvent()
        {
            bool hasNext = (eventMessageCount < messages.Length);

            if (!onceOnly && !hasNext)
            {
                eventMessageCount = 0;
            }

            return hasNext;
        }

        public override StudentPersonal GetNextResponse()
        {
            StudentPersonal studentPersonal = (StudentPersonal)sifParser.Parse(messages[responseMessageCount++]);
            responseMessageCount++;
            return studentPersonal;
        }

        public override bool HasNextResponse()
        {
            bool hasNext = (responseMessageCount < messages.Length);

            if (!onceOnly && !hasNext)
            {
                responseMessageCount = 0;
            }

            return hasNext;
        }

    }

}
