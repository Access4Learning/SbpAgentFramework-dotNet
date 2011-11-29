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
using NHibernate;
using NHibernate.Criterion;
using Systemic.Sif.Sbp.Framework.Model;

namespace Systemic.Sif.Sbp.Framework.Persistence.NHibernate
{

    public class DependentObjectDao : GenericDao<DependentObject>, IDependentObjectDao
    {

        public override DependentObject RetrieveById(long objId)
        {

            using (ISession session = NHibernateHelper.OpenSession())
            {
                DependentObject dependentObject = session.Get<DependentObject>(objId);
                NHibernateUtil.Initialize(dependentObject.ParentObjects);
                return dependentObject;
            }

        }

        public override ICollection<DependentObject> RetrieveAll()
        {

            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICollection<DependentObject> dependentObjects = session.CreateCriteria(typeof(DependentObject)).List<DependentObject>();

                foreach (DependentObject dependentObject in dependentObjects)
                {
                    NHibernateUtil.Initialize(dependentObject.ParentObjects);
                }

                return dependentObjects;
            }

        }

        public void DeleteOrphans()
        {
            IList<DependentObject> dependentObjects;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                dependentObjects = session
                    .CreateCriteria(typeof(DependentObject))
                    .Add(Restrictions.IsEmpty("ParentObjects"))
                    .List<DependentObject>();
            }

            foreach (DependentObject dependentObject in dependentObjects)
            {
                this.Delete(dependentObject);
            }

        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Persistence.IDependentObjectDao.Retrieve(string, string, string, string)">Retrieve</see>
        public DependentObject Retrieve(string sifObjectName, string objectKeyValue, string applicationId, string zoneId)
        {

            if (String.IsNullOrEmpty(sifObjectName))
            {
                throw new ArgumentException("sifObjectName parameter is null or empty.", "sifObjectName");
            }

            if (String.IsNullOrEmpty(objectKeyValue))
            {
                throw new ArgumentException("objectKeyValue parameter is null or empty.", "objectKeyValue");
            }

            if (String.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentException("applicationId parameter is null or empty.", "applicationId");
            }

            if (String.IsNullOrEmpty(zoneId))
            {
                throw new ArgumentException("zoneId parameter is null or empty.", "zoneId");
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<DependentObject> dependentObjects = session
                    .CreateCriteria(typeof(DependentObject))
                    .Add(Restrictions.Eq("SifObjectName", sifObjectName))
                    .Add(Restrictions.Eq("ObjectKeyValue", objectKeyValue))
                    .Add(Restrictions.Eq("ApplicationId", applicationId))
                    .Add(Restrictions.Eq("ZoneId", zoneId))
                    .List<DependentObject>();

                if (dependentObjects.Count > 1)
                {
                    throw new PersistenceException("Duplicate dependent object entries exist.");
                }

                return dependentObjects.FirstOrDefault();
            }

        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Persistence.IDependentObjectDao.RetrieveNotYetRequested(string, string, string)">RetrieveNotYetRequested</see>
        public ICollection<DependentObject> RetrieveNotYetRequested(string sifObjectName, string applicationId, string zoneId)
        {

            if (String.IsNullOrEmpty(sifObjectName))
            {
                throw new ArgumentException("sifObjectName parameter is null or empty.", "sifObjectName");
            }

            if (String.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentException("applicationId parameter is null or empty.", "applicationId");
            }

            if (String.IsNullOrEmpty(zoneId))
            {
                throw new ArgumentException("zoneId parameter is null or empty.", "zoneId");
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<DependentObject> dependentObjects = session
                    .CreateCriteria(typeof(DependentObject))
                    .Add(Restrictions.Eq("SifObjectName", sifObjectName))
                    .Add(Restrictions.Eq("ApplicationId", applicationId))
                    .Add(Restrictions.Eq("ZoneId", zoneId))
                    .Add(Restrictions.Eq("Requested", false))
                    .List<DependentObject>();
                return dependentObjects;
            }

        }

    }

}
