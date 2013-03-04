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
using Systemic.Sif.Sbp.Framework.Model;
using Systemic.Sif.Sbp.Framework.Persistence;
using Systemic.Sif.Sbp.Framework.Persistence.NHibernate;

namespace Systemic.Sif.Sbp.Framework.Service
{

    /// <summary>
    /// This class contains operations used by the synchronisation functionality.
    /// </summary>
    public class SyncService : ISyncService
    {
        private static readonly Object locker = new Object();

        private IObjectZoneSyncDao objectZoneSyncDao = new ObjectZoneSyncDao();

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.ISyncService.MarkAsSynced(string, string, string)">MarkAsSynced</see>
        public void MarkAsSynced(string sifObjectName, string agentId, string zoneId)
        {

            lock (locker)
            {
                ObjectZoneSync objectZoneSync = objectZoneSyncDao.Retrieve(sifObjectName, agentId, zoneId);

                if (objectZoneSync == null)
                {
                    objectZoneSync = new ObjectZoneSync { SifObjectName = sifObjectName, ZoneId = zoneId, AgentId = agentId };
                }

                objectZoneSync.LastRequested = DateTime.Now;
                objectZoneSyncDao.Save(objectZoneSync);
            }

        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.ISyncService.IsSyncRequired(string, string, string)">IsSyncRequired</see>
        public bool IsSyncRequired(string sifObjectName, string agentId, string zoneId)
        {
            return objectZoneSyncDao.Retrieve(sifObjectName, agentId, zoneId) == null;
        }

    }

}
