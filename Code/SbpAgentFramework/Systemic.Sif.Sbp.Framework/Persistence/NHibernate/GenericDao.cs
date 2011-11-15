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
