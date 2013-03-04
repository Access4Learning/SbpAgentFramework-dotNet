/*
* Copyright 2011-2013 Systemic Pty Ltd
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
using NHibernate;

namespace Systemic.Sif.Sbp.Framework.Persistence.NHibernate
{

    public class GenericDao<T> : IGenericDao<T> where T : IPersistable, new()
    {

        /// <see cref="Systemic.Sif.Sbp.Framework.Persistence.IGenericDao{T}.Save(T)">Save</see>
        public virtual void Save(T obj)
        {

            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(obj);
                    transaction.Commit();
                }

            }

        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Persistence.IGenericDao{T}.Delete(T)">Delete</see>
        public virtual void Delete(T obj)
        {

            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(obj);
                    transaction.Commit();
                }

            }

        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Persistence.IGenericDao{T}.RetrieveById(long)">RetrieveById</see>
        public virtual T RetrieveById(long objId)
        {

            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Get<T>(objId);
            }

        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Persistence.IGenericDao{T}.RetrieveAll()">RetrieveAll</see>
        public virtual ICollection<T> RetrieveAll()
        {

            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.CreateCriteria(typeof(T)).List<T>();
            }

        }

    }

}
