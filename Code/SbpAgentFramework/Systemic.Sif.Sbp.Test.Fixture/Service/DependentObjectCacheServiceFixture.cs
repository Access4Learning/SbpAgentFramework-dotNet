/*
* Copyright 2011 Systemic Pty Ltd
* 
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*    http://www.apache.org/licenses/LICENSE-2.0
* 
* Unless required by applicable law or agreed to in writing, software distributed under the License 
* is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
* or implied.
* See the License for the specific language governing permissions and limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate;
using NHibernate.Cfg;
using Systemic.Sif.Sbp.Framework.Model;
using NHibernate.Tool.hbm2ddl;
using Systemic.Sif.Sbp.Framework.Service;

namespace Systemic.Sif.Sbp.Test.Fixture.Service
{

    [TestFixture]
    class DependentObjectCacheServiceFixture
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ISessionFactory sessionFactory;
        private Configuration configuration;

        private readonly DependentObject[] dependentObjects = new[]
        {
            new DependentObject { SifObjectName = "StudentPersonal", ObjectKeyValue = "@RefId=7C834EA9EDA12090347F83297E1C290C", ZoneId = "SIFDemo", ApplicationId = "SbpDemo" }
        };

        private void CreateInitialData()
        {

            using (ISession session = sessionFactory.OpenSession())
            {

                using (ITransaction transaction = session.BeginTransaction())
                {

                    foreach (DependentObject dependentObject in dependentObjects)
                    {
                        session.Save(dependentObject);
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
        public void RetrieveDependentObject()
        {
            DependentObject retrievedObject = (new DependentObjectCacheService()).RetrieveDependentObject("StudentPersonal", "@RefId=7C834EA9EDA12090347F83297E1C290C", "SbpDemo", "SIFDemo");
            if (log.IsDebugEnabled) log.Debug("Retrieved object: " + retrievedObject);
            Console.WriteLine("Retrieved object: " + retrievedObject);
            Assert.AreEqual("StudentPersonal", retrievedObject.SifObjectName);
            Assert.AreEqual("@RefId=7C834EA9EDA12090347F83297E1C290C", retrievedObject.ObjectKeyValue);
            Assert.AreEqual("SbpDemo", retrievedObject.ApplicationId);
            Assert.AreEqual("SIFDemo", retrievedObject.ZoneId);
        }

    }

}
