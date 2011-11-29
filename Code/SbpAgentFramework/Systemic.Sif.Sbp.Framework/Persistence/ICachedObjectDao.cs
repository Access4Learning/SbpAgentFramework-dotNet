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

using System.Collections.Generic;
using Systemic.Sif.Sbp.Framework.Model;

namespace Systemic.Sif.Sbp.Framework.Persistence
{

    public interface ICachedObjectDao : IGenericDao<CachedObject>
    {

        /// <summary>
        /// Retrieve a cached SIF Object instance based upon the passed parameters.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type for the cached object.</param>
        /// <param name="flatKey">Unique "flattened" key for the cached object.</param>
        /// <param name="applicationId">Application associated with the cached object.</param>
        /// <param name="zoneId">Zone associated with the cached object.</param>
        /// <returns>A CachedObject instance based upon the passed parameters if found; null otherwise.</returns>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        /// <exception cref="Systemic.Sif.Sbp.Framework.Persistence.PersistenceException">Duplicate cached object entries exist.</exception>
        CachedObject Retrieve(string sifObjectName, string flatKey, string applicationId, string zoneId);

        /// <summary>
        /// Retrieve all cached objects of a SIF Object type for a particular application and agent that have no remaining dependencies.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type for the cached object.</param>
        /// <param name="applicationId">Application associated with the cached object.</param>
        /// <param name="agentId">Agent associated with the cached object.</param>
        /// <returns>Cached objects with no dependencies if they exist; empty collection otherwise.</returns>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        ICollection<CachedObject> RetrieveByNoDependencies(string sifObjectName, string applicationId, string agentId);

        /// <summary>
        /// Retrieve all cached objects that have remaining dependencies and an expiry date older than now.
        /// </summary>
        /// <param name="applicationId">Application associated with the expired object.</param>
        /// <param name="agentId">Agent associated with the expired object.</param>
        /// <returns>Cached objects with dependencies and past expiry date if they exist; empty collection otherwise.</returns>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        ICollection<CachedObject> RetrieveExpiredObjects(string applicationId, string agentId);

    }

}
