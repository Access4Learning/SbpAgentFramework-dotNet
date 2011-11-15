using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Systemic.Sif.Sbp.Framework.Model;
using Systemic.Sif.Sbp.Framework.Persistence;
using Systemic.Sif.Sbp.Framework.Persistence.NHibernate;

namespace Systemic.Sif.Sbp.Test.Fixture.Persistence
{

    [TestFixture]
    class ObjectZoneSyncDaoFixture
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
        public void Save()
        {
            ObjectZoneSync savedObject = new ObjectZoneSync { SifObjectName = "StudentPersonal", ZoneId = "SbpTestZone", AgentId = "SbpTestAgent", LastRequested = new DateTime(2011, 10, 3) };
            (new ObjectZoneSyncDao()).Save(savedObject);
            if (log.IsDebugEnabled) log.Debug("Saved object: " + savedObject);
            Console.WriteLine("Saved object: " + savedObject);

            using (ISession session = sessionFactory.OpenSession())
            {
                ObjectZoneSync comparisonObject = session.Get<ObjectZoneSync>(savedObject.Id);
                if (log.IsDebugEnabled) log.Debug("Comparison object: " + comparisonObject);
                Console.WriteLine("Comparison object: " + comparisonObject);
                Assert.IsNotNull(comparisonObject);
                Assert.AreNotSame(savedObject, comparisonObject);
                Assert.AreEqual(savedObject.SifObjectName, comparisonObject.SifObjectName);
                Assert.AreEqual(savedObject.ZoneId, comparisonObject.ZoneId);
                Assert.AreEqual(savedObject.AgentId, comparisonObject.AgentId);
                Assert.AreEqual(savedObject.LastRequested, comparisonObject.LastRequested);
            }

        }

        [Test]
        public void Delete()
        {
            ObjectZoneSync deletedObject = objectZoneSyncs[0];
            (new ObjectZoneSyncDao()).Delete(deletedObject);
            if (log.IsDebugEnabled) log.Debug("Deleted object: " + deletedObject);
            Console.WriteLine("Deleted object: " + deletedObject);

            using (ISession session = sessionFactory.OpenSession())
            {
                ICollection<ObjectZoneSync> objs = session.CreateCriteria(typeof(ObjectZoneSync)).List<ObjectZoneSync>();
                Assert.AreEqual(objectZoneSyncs.Length - 1, objs.Count);
            }

        }

        [Test]
        public void RetrieveById()
        {
            ObjectZoneSync retrievedObject = (new ObjectZoneSyncDao()).RetrieveById((long)1);
            if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
            Console.WriteLine("Retrieved object: " + retrievedObject);

            using (ISession session = sessionFactory.OpenSession())
            {
                ObjectZoneSync comparisonObject = session.Get<ObjectZoneSync>((long)1);
                if (log.IsDebugEnabled) log.Debug("Comparison object: " + comparisonObject);
                Console.WriteLine("Comparison object: " + comparisonObject);
                Assert.IsNotNull(comparisonObject);
                Assert.AreNotSame(retrievedObject, comparisonObject);
                Assert.AreEqual(retrievedObject.SifObjectName, comparisonObject.SifObjectName);
                Assert.AreEqual(retrievedObject.ZoneId, comparisonObject.ZoneId);
                Assert.AreEqual(retrievedObject.AgentId, comparisonObject.AgentId);
                Assert.AreEqual(retrievedObject.LastRequested, comparisonObject.LastRequested);
            }

        }

        [Test]
        public void RetrieveAll()
        {
            ICollection<ObjectZoneSync> retrievedObjects = (new ObjectZoneSyncDao()).RetrieveAll();

            foreach (ObjectZoneSync retrievedObject in retrievedObjects)
            {
                if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
                Console.WriteLine("Retrieved object: " + retrievedObject);
            }

            Assert.AreEqual(objectZoneSyncs.Length, retrievedObjects.Count);
        }

        [Test]
        public void Retrieve()
        {
            ObjectZoneSync retrievedObject = (new ObjectZoneSyncDao()).Retrieve("SchoolInfo", "SbpTestAgent", "SbpTestZone");
            if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
            Console.WriteLine("Retrieved object: " + retrievedObject);
            Assert.AreEqual("SchoolInfo", retrievedObject.SifObjectName);
            Assert.AreEqual("SbpTestZone", retrievedObject.ZoneId);
            Assert.AreEqual("SbpTestAgent", retrievedObject.AgentId);
        }

        [Test]
        [ExpectedException(typeof(PersistenceException))]
        public void RetrieveFail()
        {

            using (ISession session = sessionFactory.OpenSession())
            {
                ObjectZoneSync addedObject = new ObjectZoneSync { SifObjectName = "SchoolInfo", ZoneId = "SbpTestZone", AgentId = "SbpTestAgent", LastRequested = new DateTime(2011, 10, 4) };
                session.Save(addedObject);
            }

            ObjectZoneSync retrievedObject = (new ObjectZoneSyncDao()).Retrieve("SchoolInfo", "SbpTestAgent", "SbpTestZone");
        }

        [Test]
        public void RetrieveNull()
        {
            ObjectZoneSync retrievedObject = (new ObjectZoneSyncDao()).Retrieve("FakeSifObject", "SbpTestAgent", "SbpTestZone");
            Assert.IsNull(retrievedObject);
        }

    }

}
