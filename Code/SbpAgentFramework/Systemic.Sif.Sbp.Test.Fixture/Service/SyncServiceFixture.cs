using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Systemic.Sif.Sbp.Framework.Model;
using Systemic.Sif.Sbp.Framework.Service;

namespace Systemic.Sif.Sbp.Test.Fixture.Service
{

    [TestFixture]
    class SyncServiceFixture
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ISessionFactory sessionFactory;
        private Configuration configuration;

        private readonly ObjectZoneSync[] objectZoneSyncs = new[]
        {
            new ObjectZoneSync { SifObjectName = "StudentSchoolEnrolment", ZoneId = "SbpTestZone", AgentId = "SbpTestAgent", LastRequested = new DateTime(2011, 10, 1) },
            new ObjectZoneSync { SifObjectName = "SchoolInfo", ZoneId = "SbpTestZone", AgentId = "SbpTestAgent", LastRequested = new DateTime(2011, 10, 2) }
        };

        private void CreateInitialData()
        {

            using (ISession session = sessionFactory.OpenSession())
            {

                using (ITransaction transaction = session.BeginTransaction())
                {

                    foreach (ObjectZoneSync objectZoneSync in objectZoneSyncs)
                    {
                        session.Save(objectZoneSync);
                    }

                    transaction.Commit();
                }

            }

        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Console.WriteLine("Building session factory...");
            configuration = new Configuration();
            configuration.Configure();
            //configuration.AddAssembly(typeof(ObjectZoneSync).Assembly);
            sessionFactory = configuration.BuildSessionFactory();
        }

        [SetUp]
        public void SetupContext()
        {
            Console.WriteLine("Running schema export...");
            new SchemaExport(configuration).Execute(true, true, false);
            Console.WriteLine("Creating initial data...");
            CreateInitialData();
            Console.WriteLine("Running tests...");
        }

        [Test]
        public void MarkAsSyncedExisting()
        {
            (new SyncService()).MarkAsSynced("StudentSchoolEnrolment", "SbpTestAgent", "SbpTestZone");

            using (ISession session = sessionFactory.OpenSession())
            {
                ObjectZoneSync markedObject = session.Get<ObjectZoneSync>((long)1);
                if (log.IsDebugEnabled) log.Debug("Marked object: " + markedObject);
                Console.WriteLine("Marked object: " + markedObject);
                Assert.IsNotNull(markedObject);
                Assert.AreEqual(markedObject.SifObjectName, "StudentSchoolEnrolment");
                Assert.AreEqual(markedObject.ZoneId, "SbpTestZone");
                Assert.AreEqual(markedObject.AgentId, "SbpTestAgent");
                Assert.AreNotEqual(markedObject.LastRequested, new DateTime(2011, 10, 1));
            }

        }

        [Test]
        public void MarkAsSyncedNew()
        {
            (new SyncService()).MarkAsSynced("StudentPersonal", "SbpTestAgent", "SbpTestZone");

            using (ISession session = sessionFactory.OpenSession())
            {
                ObjectZoneSync markedObject = session.Get<ObjectZoneSync>((long)3);
                if (log.IsDebugEnabled) log.Debug("Marked object: " + markedObject);
                Console.WriteLine("Marked object: " + markedObject);
                Assert.IsNotNull(markedObject);
                Assert.AreEqual(markedObject.SifObjectName, "StudentPersonal");
                Assert.AreEqual(markedObject.ZoneId, "SbpTestZone");
                Assert.AreEqual(markedObject.AgentId, "SbpTestAgent");
                Assert.IsNotNull(markedObject.LastRequested);
            }

        }

        [Test]
        public void IsSyncRequiredTrue()
        {
            Assert.IsTrue((new SyncService()).IsSyncRequired("StudentPersonal", "SbpTestAgent", "SbpTestZone"));
        }

        [Test]
        public void IsSyncRequiredFalse()
        {
            Assert.IsFalse((new SyncService()).IsSyncRequired("StudentSchoolEnrolment", "SbpTestAgent", "SbpTestZone"));
        }

    }

}
