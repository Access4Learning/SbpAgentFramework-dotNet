using System;
using Systemic.Sif.Sbp.Framework.Model;
using Systemic.Sif.Sbp.Framework.Persistence;
using Systemic.Sif.Sbp.Framework.Persistence.NHibernate;

namespace Systemic.Sif.Sbp.Framework.Service
{

    public class SyncService : ISyncService
    {
        private IObjectZoneSyncDao objectZoneSyncDao = new ObjectZoneSyncDao();

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.ISyncService.MarkAsSynced(string, string, string)">MarkAsSynced</see>
        public void MarkAsSynced(string sifObjectName, string agentId, string zoneId)
        {
            ObjectZoneSync objectZoneSync = objectZoneSyncDao.Retrieve(sifObjectName, agentId, zoneId);

            if (objectZoneSync == null)
            {
                objectZoneSync = new ObjectZoneSync { SifObjectName = sifObjectName, ZoneId = zoneId, AgentId = agentId };
            }

            objectZoneSync.LastRequested = DateTime.Now;
            objectZoneSyncDao.Save(objectZoneSync);
        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.ISyncService.IsSyncRequired(string, string, string)">IsSyncRequired</see>
        public bool IsSyncRequired(string sifObjectName, string agentId, string zoneId)
        {
            return objectZoneSyncDao.Retrieve(sifObjectName, agentId, zoneId) == null;
        }

    }

}
