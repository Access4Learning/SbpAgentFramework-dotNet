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

using OpenADK.Library;
using OpenADK.Library.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Subscriber
{

    /// <summary>
    /// Subscribers of SIF Data Objects that are NOT defined as part of the SIF Baseline Profile (SBP) must extend this
    /// class rather than CachingSubscriber. This class still supports the functionality of controlling start-up
    /// sequencing and management through a database (much like the CachingSubscriber), but does not have the Dependent
    /// Object Cache (DOC) behind the scenes. This means objects received by this Subscriber will be processed
    /// immediately rather then checking for dependencies.
    /// Subscribers of SIF Data Objects that are defined as part of the SIF Baseline Profile (SBP) may also extend this
    /// class if they do not require the DOC.
    /// </summary>
    /// <typeparam name="T">The type of SIF Data Object this Subscriber processes, e.g. StudentPersonal, Schoolnfo, etc.</typeparam>
    public abstract class NonCachingSubscriber<T> : SyncSubscriber<T> where T : SifDataObject, new()
    {

        public NonCachingSubscriber()
            : base()
        {
        }

        public NonCachingSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
