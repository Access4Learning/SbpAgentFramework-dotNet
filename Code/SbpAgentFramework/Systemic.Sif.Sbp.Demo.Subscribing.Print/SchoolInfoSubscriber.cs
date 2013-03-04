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

using System.Collections.Generic;
using OpenADK.Library;
using OpenADK.Library.au.School;
using Systemic.Sif.Framework.Model;

namespace Systemic.Sif.Sbp.Demo.Subscribing.Print
{

    /// <summary>
    /// Subscriber of SchoolInfo SIF Data Objects.
    /// </summary>
    class SchoolInfoSubscriber : Systemic.Sif.Sbp.Framework.Subscriber.Baseline.SchoolInfoSubscriber
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IList<string> receivedSifRefIds = new List<string>();

        /// <summary>
        /// Process an event for the SchoolInfo SIF Object.
        /// </summary>
        /// <param name="sifEvent">SchoolInfo event received.</param>
        /// <param name="zone">Zone used.</param>
        protected override void ProcessEvent(SifEvent<SchoolInfo> sifEvent, IZone zone)
        {

            if (!receivedSifRefIds.Contains(sifEvent.SifDataObject.RefId))
            {
                receivedSifRefIds.Add(sifEvent.SifDataObject.RefId);
            }

            if (log.IsDebugEnabled) log.Debug("Received a " + sifEvent.EventAction.ToString() + " event for SchoolInfo in Zone " + zone.ZoneId + " with a SifRefId of " + sifEvent.SifDataObject.RefId + ":\n" + sifEvent.SifDataObject.ToXml());
        }

        /// <summary>
        /// Process a response (of a request) for an SchoolInfo SIF Object.
        /// </summary>
        /// <param name="sifDataObject">SchoolInfo response received.</param>
        /// <param name="zone">Zone used.</param>
        protected override void ProcessResponse(SchoolInfo sifDataObject, IZone zone)
        {
            if (log.IsDebugEnabled) log.Debug("Received a request response for SchoolInfo in Zone " + zone.ZoneId + ":\n" + sifDataObject.ToXml());
        }

    }

}
