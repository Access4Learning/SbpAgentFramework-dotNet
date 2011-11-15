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
    class DependentObjectDaoFixture
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ISessionFactory sessionFactory;
        private Configuration configuration;
        private DependentObject[] dependentObjects;

        private void CreateInitialData()
        {
            CachedObject parentObject = new CachedObject();

            dependentObjects = new[]
            {
                new DependentObject
                {
                    SifObjectName = "StudentPersonal", ObjectKeyValue = "st1", ApplicationId = "app1", ZoneId = "zone1", ParentObjects = new Collection<CachedObject>() { parentObject }
                },
                new DependentObject
                {
                    SifObjectName = "StudentPersonal", ObjectKeyValue = "st2", ApplicationId = "app2", ZoneId = "zone2", ParentObjects = new Collection<CachedObject>() { parentObject }
                },
                new DependentObject { SifObjectName = "StudentPersonal", ObjectKeyValue = "st3", ApplicationId = "app3", ZoneId = "zone3", Requested = true },
                new DependentObject { SifObjectName = "StudentPersonal", ObjectKeyValue = "st4", ApplicationId = "app3", ZoneId = "zone3", Requested = true },
                new DependentObject { SifObjectName = "StudentPersonal", ObjectKeyValue = "st5", ApplicationId = "app3", ZoneId = "zone3", }
            };

            using (ISession session = sessionFactory.OpenSession())
            {

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(parentObject);

                    foreach (DependentObject cachedObject in dependentObjects)
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
            //configuration.AddAssembly(typeof(DependentObject).Assembly);
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
            DependentObject savedObject = new DependentObject { SifObjectName = "StudentAttendance", ObjectKeyValue = "key3" };
            (new DependentObjectDao()).Save(savedObject);
            if (log.IsDebugEnabled) log.Debug("Saved object: " + savedObject);
            Console.WriteLine("Saved object: " + savedObject);

            using (ISession session = sessionFactory.OpenSession())
            {
                DependentObject comparisonObject = session.Get<DependentObject>(savedObject.Id);
                if (log.IsDebugEnabled) log.Debug("Comparison object: " + comparisonObject);
                Console.WriteLine("Comparison object: " + comparisonObject);
                Assert.IsNotNull(comparisonObject);
                Assert.AreNotSame(savedObject, comparisonObject);
                Assert.AreEqual(savedObject.SifObjectName, comparisonObject.SifObjectName);
                Assert.AreEqual(savedObject.ObjectKeyValue, comparisonObject.ObjectKeyValue);
            }

        }

        [Test]
        public void Delete()
        {
            DependentObject deletedObject = dependentObjects[0];
            (new DependentObjectDao()).Delete(deletedObject);
            if (log.IsDebugEnabled) log.Debug("Deleted object: " + deletedObject);
            Console.WriteLine("Deleted object: " + deletedObject);

            using (ISession session = sessionFactory.OpenSession())
            {
                ICollection<DependentObject> objs = session.CreateCriteria(typeof(DependentObject)).List<DependentObject>();
                Assert.AreEqual(dependentObjects.Length - 1, objs.Count);
            }

        }

        [Test]
        public void RetrieveById()
        {
            DependentObject retrievedObject = (new DependentObjectDao()).RetrieveById((long)1);
            if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
            Console.WriteLine("Retrieved object: " + retrievedObject);

            using (ISession session = sessionFactory.OpenSession())
            {
                DependentObject comparisonObject = session.Get<DependentObject>((long)1);
                if (log.IsDebugEnabled) log.Debug("Comparison object: " + comparisonObject);
                Console.WriteLine("Comparison object: " + comparisonObject);
                Assert.IsNotNull(comparisonObject);
                Assert.AreNotSame(retrievedObject, comparisonObject);
                Assert.AreEqual(retrievedObject.SifObjectName, comparisonObject.SifObjectName);
                Assert.AreEqual(retrievedObject.ObjectKeyValue, comparisonObject.ObjectKeyValue);
            }

        }

        [Test]
        public void RetrieveAll()
        {
            ICollection<DependentObject> retrievedObjects = (new DependentObjectDao()).RetrieveAll();

            foreach (DependentObject retrievedObject in retrievedObjects)
            {
                if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
                Console.WriteLine("Retrieved object: " + retrievedObject);
            }

            Assert.AreEqual(dependentObjects.Length, retrievedObjects.Count);
        }

        [Test]
        public void DeleteOrphans()
        {
            (new DependentObjectDao()).DeleteOrphans();

            using (ISession session = sessionFactory.OpenSession())
            {
                ICollection<DependentObject> objs = session.CreateCriteria(typeof(DependentObject)).List<DependentObject>();
                Assert.AreEqual(dependentObjects.Length - 3, objs.Count);
            }

        }

        [Test]
        public void Retrieve()
        {
            DependentObject retrievedObject = (new DependentObjectDao()).Retrieve("StudentPersonal", "st1", "app1", "zone1");
            if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
            Console.WriteLine("Retrieved object: " + retrievedObject);
            Assert.AreEqual("StudentPersonal", retrievedObject.SifObjectName);
            Assert.AreEqual("st1", retrievedObject.ObjectKeyValue);
            Assert.AreEqual("app1", retrievedObject.ApplicationId);
            Assert.AreEqual("zone1", retrievedObject.ZoneId);
        }

        [Test]
        [ExpectedException(typeof(PersistenceException))]
        public void RetrieveFail()
        {

            using (ISession session = sessionFactory.OpenSession())
            {
                DependentObject addedObject = new DependentObject { SifObjectName = "StudentPersonal", ObjectKeyValue = "st1", ApplicationId = "app1", ZoneId = "zone1" };
                session.Save(addedObject);
            }

            DependentObject retrievedObject = (new DependentObjectDao()).Retrieve("StudentPersonal", "st1", "app1", "zone1");
        }

        [Test]
        public void RetrieveNull()
        {
            DependentObject retrievedObject = (new DependentObjectDao()).Retrieve("FakeSifObject", "key0", "app0", "zone0");
            Assert.IsNull(retrievedObject);
        }

        [Test]
        public void RetrieveNotYetRequested()
        {
            ICollection<DependentObject> retrievedObjects = (new DependentObjectDao()).RetrieveNotYetRequested("StudentPersonal", "app3", "zone3");

            foreach (DependentObject retrievedObject in retrievedObjects)
            {
                if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
                Console.WriteLine("Retrieved object: " + retrievedObject);
            }

            Assert.AreEqual(1, retrievedObjects.Count);
            Assert.AreEqual("st5", retrievedObjects.First().ObjectKeyValue);
        }

    }

}
