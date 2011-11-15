using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    class CachedObjectDaoFixture
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ISessionFactory sessionFactory;
        private Configuration configuration;
        private CachedObject[] cachedObjects;

        private void CreateInitialData()
        {
            DependentObject dependentObject1 = new DependentObject { SifObjectName = "StudentPersonal", ObjectKeyValue = "st1" };
            DependentObject dependentObject2 = new DependentObject { SifObjectName = "StudentPersonal", ObjectKeyValue = "st2" };

            cachedObjects = new[]
            {
                new CachedObject
                {
                    SifObjectName = "TeachingGroup", ObjectKeyValue = "tg1", ApplicationId = "app1", ZoneId = "zone1", DependentObjects = new Collection<DependentObject>
                    {
                        dependentObject1,
                        dependentObject2
                    }
                },
                new CachedObject
                {
                    SifObjectName = "StudentSchoolEnrolment", ObjectKeyValue = "se1", DependentObjects = new Collection<DependentObject>
                    {
                        dependentObject1
                    }
                },
                new CachedObject
                {
                    SifObjectName = "StudentPersonal", ObjectKeyValue = "sp1", AgentId = "agent", ApplicationId = "app", ExpiryDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)), DependentObjects = new Collection<DependentObject>
                    {
                        new DependentObject { SifObjectName = "DummyObject1", ObjectKeyValue = "do1" }
                    }
                },
                new CachedObject
                {
                    SifObjectName = "StudentPersonal", ObjectKeyValue = "sp2", AgentId = "agent", ApplicationId = "app", ExpiryDate = DateTime.Now.AddDays(1), DependentObjects = new Collection<DependentObject>
                    {
                        new DependentObject { SifObjectName = "DummyObject2", ObjectKeyValue = "do2" }
                    }
                },
                new CachedObject { SifObjectName = "StudentPersonal", ObjectKeyValue = "sp3", AgentId = "agent", ApplicationId = "app" }
            };

            using (ISession session = sessionFactory.OpenSession())
            {

                using (ITransaction transaction = session.BeginTransaction())
                {

                    foreach (CachedObject cachedObject in cachedObjects)
                    {
                        session.Save(cachedObject);
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
            //configuration.AddAssembly(typeof(CachedObject).Assembly);
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

            ICollection<DependentObject> depedentObjects = new Collection<DependentObject>
            {
                new DependentObject { SifObjectName = "AttendanceSummary", ObjectKeyValue = "key3" },
                new DependentObject { SifObjectName = "StudentDailyAttendance", ObjectKeyValue = "key4" }
            };

            CachedObject savedObject = new CachedObject { SifObjectName = "StudentAttendance", DependentObjects = depedentObjects };
            (new CachedObjectDao()).Save(savedObject);
            if (log.IsDebugEnabled) log.Debug("Saved object: " + savedObject);
            Console.WriteLine("Saved object: " + savedObject);

            using (ISession session = sessionFactory.OpenSession())
            {
                CachedObject comparisonObject = session.Get<CachedObject>(savedObject.Id);
                if (log.IsDebugEnabled) log.Debug("Comparison object: " + comparisonObject);
                Console.WriteLine("Comparison object: " + comparisonObject);
                Assert.IsNotNull(comparisonObject);
                Assert.AreNotSame(savedObject, comparisonObject);
                Assert.AreEqual(savedObject.SifObjectName, comparisonObject.SifObjectName);
                Assert.AreEqual(savedObject.ZoneId, comparisonObject.ZoneId);
                Assert.AreEqual(savedObject.AgentId, comparisonObject.AgentId);
            }

        }

        [Test]
        public void Delete()
        {
            CachedObject deletedObject = cachedObjects[0];
            (new CachedObjectDao()).Delete(deletedObject);
            if (log.IsDebugEnabled) log.Debug("Deleted object: " + deletedObject);
            Console.WriteLine("Deleted object: " + deletedObject);

            using (ISession session = sessionFactory.OpenSession())
            {
                ICollection<CachedObject> objs = session.CreateCriteria(typeof(CachedObject)).List<CachedObject>();
                Assert.AreEqual(cachedObjects.Length - 1, objs.Count);
            }

        }

        [Test]
        public void RetrieveById()
        {
            CachedObject retrievedObject = (new CachedObjectDao()).RetrieveById((long)1);
            if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
            Console.WriteLine("Retrieved object: " + retrievedObject);

            using (ISession session = sessionFactory.OpenSession())
            {
                CachedObject comparisonObject = session.Get<CachedObject>((long)1);
                if (log.IsDebugEnabled) log.Debug("Comparison object: " + comparisonObject);
                Console.WriteLine("Comparison object: " + comparisonObject);
                Assert.IsNotNull(comparisonObject);
                Assert.AreNotSame(retrievedObject, comparisonObject);
                Assert.AreEqual(retrievedObject.SifObjectName, comparisonObject.SifObjectName);
                Assert.AreEqual(retrievedObject.ZoneId, comparisonObject.ZoneId);
                Assert.AreEqual(retrievedObject.AgentId, comparisonObject.AgentId);
            }

        }

        [Test]
        public void RetrieveAll()
        {
            ICollection<CachedObject> retrievedObjects = (new CachedObjectDao()).RetrieveAll();

            foreach (CachedObject retrievedObject in retrievedObjects)
            {
                if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
                Console.WriteLine("Retrieved object: " + retrievedObject);
            }

            Assert.AreEqual(cachedObjects.Length, retrievedObjects.Count);
        }

        [Test]
        public void Retrieve()
        {
            CachedObject retrievedObject = (new CachedObjectDao()).Retrieve("TeachingGroup", "tg1", "app1", "zone1");
            if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
            Console.WriteLine("Retrieved object: " + retrievedObject);
            Assert.AreEqual("TeachingGroup", retrievedObject.SifObjectName);
            Assert.AreEqual("tg1", retrievedObject.ObjectKeyValue);
            Assert.AreEqual("app1", retrievedObject.ApplicationId);
            Assert.AreEqual("zone1", retrievedObject.ZoneId);
        }

        [Test]
        [ExpectedException(typeof(PersistenceException))]
        public void RetrieveFail()
        {

            using (ISession session = sessionFactory.OpenSession())
            {
                CachedObject addedObject = new CachedObject { SifObjectName = "TeachingGroup", ObjectKeyValue = "tg1", ApplicationId = "app1", ZoneId = "zone1" };
                session.Save(addedObject);
            }

            CachedObject retrievedObject = (new CachedObjectDao()).Retrieve("TeachingGroup", "tg1", "app1", "zone1");
        }

        [Test]
        public void RetrieveNull()
        {
            CachedObject retrievedObject = (new CachedObjectDao()).Retrieve("FakeSifObject", "key0", "app0", "zone0");
            Assert.IsNull(retrievedObject);
        }

        [Test]
        public void RetrieveByNoDependencies()
        {
            ICollection<CachedObject> retrievedObjects = (new CachedObjectDao()).RetrieveByNoDependencies("StudentPersonal", "app", "agent");

            foreach (CachedObject retrievedObject in retrievedObjects)
            {
                if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
                Console.WriteLine("Retrieved object: " + retrievedObject);
            }

            Assert.AreEqual(1, retrievedObjects.Count);
            Assert.AreEqual("sp3", retrievedObjects.First().ObjectKeyValue);
        }

        [Test]
        public void RetrieveExpiredObjects()
        {
            ICollection<CachedObject> retrievedObjects = (new CachedObjectDao()).RetrieveExpiredObjects("app", "agent");

            foreach (CachedObject retrievedObject in retrievedObjects)
            {
                if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
                Console.WriteLine("Retrieved object: " + retrievedObject);
            }

            Assert.AreEqual(1, retrievedObjects.Count);
            Assert.AreEqual("sp1", retrievedObjects.First().ObjectKeyValue);
        }

    }

}
