using System.Collections.Generic;
using Edustructures.SifWorks;
using Edustructures.SifWorks.Student;
using Systemic.Sif.Framework.Model;

namespace Systemic.Sif.Sbp.Demo.Publishing.ResponseManager
{

    class StudentPersonalIterator : GenericIterator<StudentPersonal>
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static int eventMessageCount = 0;
        private static int responseMessageCount = 0;

        bool onceOnly = true;
        private SifParser sifParser = SifParser.NewInstance();
        private IList<string> eventMessages = new List<string>();
        private IList<string> responseMessages = new List<string>();
        private IDictionary<string, string> messages = new Dictionary<string, string>()
        {
            {
                "7C834EA9EDA12090347F83297E1C290C",
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
            },
            {
                "12345678901234567890123456789012",
                @"
                  <StudentPersonal RefId=""12345678901234567890123456789012"">
                    <LocalId>S1234567</LocalId>
                    <PersonInfo>
                      <Name Type=""LGL"">
                        <FamilyName>Bloggs</FamilyName>
                        <GivenName>Joe</GivenName>
                        <FullName>Joe Bloggs</FullName>
                      </Name>
                    </PersonInfo>
                  </StudentPersonal>
                "
            }
        };

        public StudentPersonalIterator(string key)
        {

            foreach (string message in messages.Values)
            {
                eventMessages.Add(message);
            }

            if (key == null)
            {

                foreach (string message in messages.Values)
                {
                    responseMessages.Add(message);
                }

            }
            else
            {
                string responseMessage;

                if (messages.TryGetValue(key, out responseMessage))
                {
                    responseMessages.Add(responseMessage);
                }

            }

        }

        public override SifEvent<StudentPersonal> GetNextEvent()
        {
            StudentPersonal studentPersonal = (StudentPersonal)sifParser.Parse(eventMessages[eventMessageCount]);
            eventMessageCount++;
            if (log.IsDebugEnabled) log.Debug("StudentPersonalIterator event data " + studentPersonal.ToXml() + ".");
            SifEvent<StudentPersonal> sifEvent = new SifEvent<StudentPersonal>(studentPersonal, EventAction.Change);
            return sifEvent;
        }

        public override bool HasNextEvent()
        {
            bool hasNext = (eventMessageCount < eventMessages.Count);

            if (!onceOnly && !hasNext)
            {
                eventMessageCount = 0;
            }

            return hasNext;
        }

        public override StudentPersonal GetNextResponse()
        {
            StudentPersonal studentPersonal = (StudentPersonal)sifParser.Parse(responseMessages[responseMessageCount]);
            if (log.IsDebugEnabled) log.Debug("StudentPersonalIterator response data " + studentPersonal.ToXml() + ".");
            responseMessageCount++;
            return studentPersonal;
        }

        public override bool HasNextResponse()
        {
            bool hasNext = (responseMessageCount < responseMessages.Count);

            if (!onceOnly && !hasNext)
            {
                responseMessageCount = 0;
            }

            return hasNext;
        }

    }

}
