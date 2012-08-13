/*
* Copyright 2012 Systemic Pty Ltd
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

namespace Systemic.Sif.Sbp.Demo.Publishing.ResponseManager
{

    /// <summary>
    /// Class used to retrieve Identity SIF Objects.
    /// </summary>
    class IdentityIterator : GenericIterator<Identity>
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static int eventMessageCount = 0;
        private static int responseMessageCount = 0;

        bool onceOnly = true;
        private SifParser sifParser = SifParser.NewInstance();
        private string[] messages = new string[]
        {
            @"
              <Identity RefId=""4286194F43ED43C18EE2F0A27C4BEF86"">
                <SIF_RefId SIF_RefObject=""StudentPersonal"">7C834EA9EDA12090347F83297E1C290C</SIF_RefId>
                <AuthenticationSource>OpenID</AuthenticationSource>
                <IdentityAssertions>
                  <IdentityAssertion SchemaName=""openid.identity"">http://verisign.org/p/alice </IdentityAssertion>
                  <IdentityAssertion SchemaName=""openid.server"">http://verisign.org </IdentityAssertion>
                </IdentityAssertions>
                <AuthenticationSourceGlobalUID>A9A6CB2BC49344278C1FD6587D448B35</AuthenticationSourceGlobalUID>
              </Identity>
            "
        };

        /// <summary>
        /// Retrieve the next change event for an Identity SIF Object.
        /// </summary>
        /// <returns>Change event for an Identity SIF Object.</returns>
        public override SifEvent<Identity> GetNextEvent()
        {
            Identity identity = (Identity)sifParser.Parse(messages[eventMessageCount]);
            eventMessageCount++;
            if (log.IsDebugEnabled) log.Debug("IdentityIterator data " + identity.ToXml() + ".");
            SifEvent<Identity> sifEvent = new SifEvent<Identity>(identity, EventAction.Change);
            return sifEvent;
        }

        /// <summary>
        /// Check if there is another change event for an Identity SIF Object.
        /// </summary>
        /// <returns>True if there is another event; false otherwise.</returns>
        public override bool HasNextEvent()
        {
            bool hasNext = (eventMessageCount < messages.Length);

            if (!onceOnly && !hasNext)
            {
                eventMessageCount = 0;
            }

            return hasNext;
        }

        /// <summary>
        /// Retrieve the next response (of a request) for an Identity SIF Object.
        /// </summary>
        /// <returns>Response for an Identity SIF Object.</returns>
        public override Identity GetNextResponse()
        {
            Identity identity = (Identity)sifParser.Parse(messages[responseMessageCount++]);
            responseMessageCount++;
            return identity;
        }

        /// <summary>
        /// Check if there is another response for an Identity SIF Object.
        /// </summary>
        /// <returns>True if there is another response; false otherwise.</returns>
        public override bool HasNextResponse()
        {
            bool hasNext = (responseMessageCount < messages.Length);

            if (!onceOnly && !hasNext)
            {
                responseMessageCount = 0;
            }

            return hasNext;
        }

    }

}
