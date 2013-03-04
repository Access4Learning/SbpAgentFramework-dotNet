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

using OpenADK.Library;
using OpenADK.Library.Tools.Cfg;
using Systemic.Sif.Framework.Subscriber;
using Systemic.Sif.Sbp.Framework.Service;

namespace Systemic.Sif.Sbp.Framework.Subscriber
{

    /// <summary>
    /// This class manages SIF requests through the use of configuration specified in database tables rather than
    /// through the default behaviour of the SIF Common Framework. It checks the database table (with MakeRequest) at
    /// specific intervals (based on RequestFrequency) to determine whether a SIF request is required.
    /// </summary>
    /// <typeparam name="T">The type of SIF Data Object this Subscriber processes, e.g. StudentPersonal, Schoolnfo.</typeparam>
    public abstract class SyncSubscriber<T> : GenericSubscriber<T> where T : SifDataObject, new()
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ISyncService syncService = new SyncService();

        /// <summary>
        /// Create an instance of the Subscriber without referencing the Agent configuration settings.
        /// </summary>
        public SyncSubscriber()
            : base()
        {
        }

        /// <summary>
        /// Create an instance of the Subscriber based upon the Agent configuration settings.
        /// </summary>
        /// <param name="agentConfig">Agent configuration settings.</param>
        /// <exception cref="System.ArgumentException">agentConfig parameter is null.</exception>
        public SyncSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

        /// <summary>
        /// This method will check the database table to determine whether a SIF request is required.
        /// </summary>
        /// <param name="zone">Zone associated with the SIF request.</param>
        /// <returns>True is a SIF request is required; false otherwise.</returns>
        protected override bool MakeRequest(IZone zone)
        {
            bool makeRequest = syncService.IsSyncRequired(SifObjectType.Name, AgentConfiguration.SourceId, zone.ZoneId);
            if (log.IsDebugEnabled) log.Debug("Make request is " + makeRequest + " for SIF Object " + SifObjectType + " and Agent " + AgentConfiguration.SourceId + " and Zone " + zone.ZoneId + ".");
            return makeRequest;
        }

        /// <summary>
        /// Override the default behaviour of the SIF Common Framework base Subscriber to mark the associated SIF Data
        /// Object type as requested once done.
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
