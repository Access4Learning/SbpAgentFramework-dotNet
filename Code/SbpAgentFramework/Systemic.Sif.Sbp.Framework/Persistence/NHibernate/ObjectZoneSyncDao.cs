using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Systemic.Sif.Sbp.Framework.Model;

namespace Systemic.Sif.Sbp.Framework.Persistence.NHibernate
{

    public class ObjectZoneSyncDao : GenericDao<ObjectZoneSync>, IObjectZoneSyncDao
    {

        /// <see cref="Systemic.Sif.Sbp.Framework.Persistence.IObjectZoneSyncDao.Retrieve(string, string, string)">Retrieve</see>
        public ObjectZoneSync Retrieve(string sifObjectName, string agentId, string zoneId)
        {

            if (String.IsNullOrEmpty(sifObjectName))
            {
                throw new ArgumentException("sifObjectName parameter is null or empty.", "sifObjectName");
            }

            if (String.IsNullOrEmpty(agentId))
            {
                throw new ArgumentException("agentId parameter is null or empty.", "agentId");
            }

            if (String.IsNullOrEmpty(zoneId))
            {
                throw new ArgumentException("zoneId parameter is null or empty.", "zoneId");
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<ObjectZoneSync> objectZoneSyncs = session
                    .CreateCriteria(typeof(ObjectZoneSync))
                    .Add(Restrictions.Eq("SifObjectName", sifObjectName))
                    .Add(Restrictions.Eq("AgentId", agentId))
                    .Add(Restrictions.Eq("ZoneId", zoneId))
                    .List<ObjectZoneSync>();

                if (objectZoneSyncs.Count > 1)
                {
                    throw new PersistenceException("Duplicate object/zone sync entries exist.");
                }

                return objectZoneSyncs.FirstOrDefault();
            }

        }

    }

}
