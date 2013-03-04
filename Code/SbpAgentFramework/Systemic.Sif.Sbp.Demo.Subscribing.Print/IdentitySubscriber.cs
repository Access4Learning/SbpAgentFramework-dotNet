/*
* Copyright 2012-2013 Systemic Pty Ltd
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
using OpenADK.Library.au.Infrastructure;
using Systemic.Sif.Framework.Model;
using Systemic.Sif.Sbp.Framework.Model.Metadata;

namespace Systemic.Sif.Sbp.Demo.Subscribing.Print
{

    /// <summary>
    /// Subscriber of Identity SIF Data Objects.
    /// </summary>
    class IdentitySubscriber : Systemic.Sif.Sbp.Framework.Subscriber.Baseline.IdentitySubscriber
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Process an event for the Identity SIF Object.
        /// </summary>
        /// <param name="sifEvent">Identity event received.</param>
        /// <param name="zone">Zone used.</param>
        protected override void ProcessEvent(SifEvent<Identity> sifEvent, IZone zone)
        {
            if (log.IsDebugEnabled) log.Debug("Received a " + sifEvent.EventAction.ToString() + " event for Identity in Zone " + zone.ZoneId + ":\n" + sifEvent.SifDataObject.ToXml());
        }

        /// <summary>
        /// Process a response (of a request) for an Identity SIF Object.
        /// </summary>
        /// <param name="sifDataObject">Identity response received.</param>
        /// <param name="zone">Zone used.</param>
        protected override void ProcessResponse(Identity sifDataObject, IZone zone)
        {
            if (log.IsDebugEnabled) log.Debug("Received a request response for Identity in Zone " + zone.ZoneId + ":\n" + sifDataObject.ToXml());
        }

        /// <summary>
        /// Determine whether a dependent object of the Identity SIF Object already exists in the target system.
        /// </summary>
        /// <param name="dependentObjectName">SIF type of the dependent object.</param>
        /// <param name="objectKeyValue">SIF RefId for the dependent object.</param>
        /// <returns></returns>
        protected override bool DoesObjectExistInTargetSystem(string dependentObjectName, string objectKeyValue)
        {
            bool exists = false;
            SifRefIdMetadata sifRefIdMetadata = new SifRefIdMetadata(objectKeyValue);

            if ("StudentPersonal".Equals(dependentObjectName) && StudentPersonalSubscriber.receivedSifRefIds.Contains(sifRefIdMetadata.Value))
            {
                if (log.IsDebugEnabled) log.Debug("StudentPersonal with a SifRefId of " + sifRefIdMetadata.Value + " exists in the target system.");
                exists = true;
            }

            return exists;
        }

    }

}
