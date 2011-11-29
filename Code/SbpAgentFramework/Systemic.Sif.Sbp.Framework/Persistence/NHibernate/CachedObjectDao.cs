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

    public class CachedObjectDao : GenericDao<CachedObject>, ICachedObjectDao
    {

        public override void Delete(CachedObject obj)
        {
            base.Delete(obj);
            (new DependentObjectDao()).DeleteOrphans();
        }

        public override CachedObject RetrieveById(long objId)
        {

            using (ISession session = NHibernateHelper.OpenSession())
            {
                CachedObject cachedObject = session.Get<CachedObject>(objId);
                //NHibernateUtil.Initialize(cachedObject.DependentObjects);
                return cachedObject;
            }

        }

        public override ICollection<CachedObject> RetrieveAll()
        {

            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICollection<CachedObject> cachedObjects = session.CreateCriteria(typeof(CachedObject)).List<CachedObject>();
                return cachedObjects;
            }

        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Persistence.ICachedObjectDao.Retrieve(string, string, string, string)">Retrieve</see>
        public CachedObject Retrieve(string sifObjectName, string flatKey, string applicationId, string zoneId)
        {

            if (String.IsNullOrEmpty(sifObjectName))
            {
                throw new ArgumentException("sifObjectName parameter is null or empty.", "sifObjectName");
            }

            if (String.IsNullOrEmpty(flatKey))
            {
                throw new ArgumentException("flatKey parameter is null or empty.", "flatKey");
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
                IList<CachedObject> cachedObjects = session
                    .CreateCriteria(typeof(CachedObject))
                    .Add(Restrictions.Eq("SifObjectName", sifObjectName))
                    .Add(Restrictions.Eq("ObjectKeyValue", flatKey))
                    .Add(Restrictions.Eq("ApplicationId", applicationId))
                    .Add(Restrictions.Eq("ZoneId", zoneId))
                    .List<CachedObject>();

                if (cachedObjects.Count > 1)
                {
                    throw new PersistenceException("Duplicate cached object entries exist.");
                }

                return cachedObjects.FirstOrDefault();
            }

        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Persistence.ICachedObjectDao.RetrieveByNoDependencies(string, string, string)">RetrieveByNoDependencies</see>
        public ICollection<CachedObject> RetrieveByNoDependencies(string sifObjectName, string applicationId, string agentId)
        {

            if (String.IsNullOrEmpty(sifObjectName))
            {
                throw new ArgumentException("sifObjectName parameter is null or empty.", "sifObjectName");
            }

            if (String.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentException("applicationId parameter is null or empty.", "applicationId");
            }

            if (String.IsNullOrEmpty(agentId))
            {
                throw new ArgumentException("agentId parameter is null or empty.", "agentId");
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<CachedObject> cachedObjects = session
                    .CreateCriteria(typeof(CachedObject))
                    .Add(Restrictions.Eq("SifObjectName", sifObjectName))
                    .Add(Restrictions.Eq("ApplicationId", applicationId))
                    .Add(Restrictions.Eq("AgentId", agentId))
                    .Add(Restrictions.IsEmpty("DependentObjects"))
                    .List<CachedObject>();
                return cachedObjects;
            }

        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Persistence.ICachedObjectDao.RetrieveExpiredObjects(string, string)">RetrieveExpiredObjects</see>
        public ICollection<CachedObject> RetrieveExpiredObjects(string applicationId, string agentId)
        {

            if (String.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentException("applicationId parameter is null or empty.", "applicationId");
            }

            if (String.IsNullOrEmpty(agentId))
            {
                throw new ArgumentException("agentId parameter is null or empty.", "agentId");
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<CachedObject> cachedObjects = session
                    .CreateCriteria(typeof(CachedObject))
                    .Add(Restrictions.Eq("ApplicationId", applicationId))
                    .Add(Restrictions.Eq("AgentId", agentId))
                    .Add(Restrictions.IsNotEmpty("DependentObjects"))
                    .Add(Restrictions.Le("ExpiryDate", DateTime.Now))
                    .List<CachedObject>();

                foreach (CachedObject cachedObject in cachedObjects)
                {
                    NHibernateUtil.Initialize(cachedObject.DependentObjects);
                }

                return cachedObjects;
            }

        }

    }

}
