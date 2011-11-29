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

using Edustructures.SifWorks;
using Edustructures.SifWorks.Tools.Cfg;
using Systemic.Sif.Framework.Subscriber;
using Systemic.Sif.Sbp.Framework.Service;

namespace Systemic.Sif.Sbp.Framework.Subscriber
{

    /// <summary>
    /// This class implements the Sync Control Functionality through configuration specified in database tables rather
    /// than through the default behaviour of the SIF Common Framework.
    /// It is important to note that if the SIF Common Framework property file turns off sync altogether, then the
    /// method in this class has no affect (meaning it will be ignored).
    /// </summary>
    /// <typeparam name="T">The type of SIF Data Object this Subscriber processes, e.g. StudentPersonal, Schoolnfo, etc.</typeparam>
    public abstract class SyncSubscriber<T> : GenericSubscriber<T> where T : SifDataObject, new()
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int requestFrequency = 3600000;
        ISyncService syncService = new SyncService();

        protected override int RequestFrequency
        {
            get { return requestFrequency; }
            set { requestFrequency = value; }
        }

        public SyncSubscriber()
            : base()
        {
        }

        public SyncSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

        protected override bool MakeRequest(IZone zone)
        {
            bool makeRequest = syncService.IsSyncRequired(SifObjectType.Name, AgentConfiguration.SourceId, zone.ZoneId);
            if (log.IsDebugEnabled) log.Debug("Make request is " + makeRequest + " for SIF Object " + SifObjectType + " and Agent " + AgentConfiguration.SourceId + " and Zone " + zone.ZoneId + ".");
            return makeRequest;
        }

        /// <summary>
        /// Override the default behaviour of the SIF Common Framework base Subscriber.
        /// </summary>
        /// <param name="zone">Zone to synchronise with.</param>
        /// <see cref="Systemic.Sif.Framework.Subscriber.GenericSubscriber{T}.BroadcastRequest(Edustructures.SifWorks.IZone)"/>
        protected override void BroadcastRequest(IZone zone)
        {
            base.BroadcastRequest(zone);
            syncService.MarkAsSynced(SifObjectType.Name, AgentConfiguration.SourceId, zone.ZoneId);
        }

    }

}
