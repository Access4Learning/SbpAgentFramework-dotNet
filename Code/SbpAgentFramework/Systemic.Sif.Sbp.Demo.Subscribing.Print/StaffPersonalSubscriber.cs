/*
* Copyright 2013 Systemic Pty Ltd
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
using OpenADK.Library.au.Student;
using Systemic.Sif.Framework.Model;

namespace Systemic.Sif.Sbp.Demo.Subscribing.Print
{

    /// <summary>
    /// Subscriber of StaffPersonal data objects.
    /// </summary>
    public class StaffPersonalSubscriber : Systemic.Sif.Sbp.Framework.Subscriber.Baseline.StaffPersonalSubscriber
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IList<string> receivedSifRefIds = new List<string>();

        /// <summary>
        /// This implementation will simply print the received StaffPersonal record to the console.
        /// </summary>
        /// <param name="sifEvent">SIF Event received.</param>
        /// <param name="zone">Zone the SIF Event was received from.</param>
        protected override void ProcessEvent(SifEvent<StaffPersonal> sifEvent, IZone zone)
        {

            if (!receivedSifRefIds.Contains(sifEvent.SifDataObject.RefId))
            {
                receivedSifRefIds.Add(sifEvent.SifDataObject.RefId);
            }

            if (log.IsDebugEnabled) log.Debug("Received a " + sifEvent.EventAction.ToString() + " event for StaffPersonal in Zone " + zone.ZoneId + " with a SifRefId of " + sifEvent.SifDataObject.RefId + ":\n" + sifEvent.SifDataObject.ToXml());
        }

        /// <summary>
        /// This implementation will simply print the received StaffPersonal record to the console.
        /// </summary>
        /// <param name="sifDataObject">StaffPersonal record received</param>
        /// <param name="zone">Zone the StaffPersonal record was received from.</param>
        protected override void ProcessResponse(StaffPersonal sifDataObject, IZone zone)
        {
            if (log.IsDebugEnabled) log.Debug("Received a request response for StaffPersonal in Zone " + zone.ZoneId + ":\n" + sifDataObject.ToXml());
        }

    }

}
