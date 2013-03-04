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
using Systemic.Sif.Sbp.Framework.Model.Metadata;

namespace Systemic.Sif.Sbp.Framework.Subscriber
{

    public abstract class CachingSubscriber<T> : SyncSubscriber<T> where T : SifDataObject, new()
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected virtual int CacheCheckFrequency
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.checkFrequency", 3600000); }
            set { }
        }

        protected virtual bool CacheEnabled
        {
            get { return agentProperties.GetProperty("subscriber." + SifObjectType.Name + ".cache.enabled", true); }
            set { }
        }

        /// <summary>
        /// Unique identfier for the application associated with the Agent for this Subscriber.
        /// </summary>
        public string ApplicationId
        {
            get { return agentProperties.GetProperty("agent.applicationId", "defaultId"); }
            set { }
        }

        /// <summary>
        /// Create an instance of the Subscriber without referencing the Agent configuration settings.
        /// </summary>
        public CachingSubscriber()
            : base()
        {
        }

        /// <summary>
        /// Create an instance of the Subscriber based upon the Agent configuration settings.
        /// </summary>
        /// <param name="agentConfig">Agent configuration settings.</param>
        /// <exception cref="System.ArgumentException">agentConfig parameter is null.</exception>
        public CachingSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

        /// <summary>
        /// This method returns metadata for the SIF Data Object associated with this Subscriber.
        /// </summary>
        /// <param name="sifDataObject">SIF Data Object associated with this Subscriber.</param>
        /// <returns>Metadata for the SIF Data Object associated with this Subscriber.</returns>
        internal virtual SifDataObjectMetadata<T> MetadataInstance(T sifDataObject)
        {
            return new SifDataObjectMetadata<T>(sifDataObject);
        }

    }

}
