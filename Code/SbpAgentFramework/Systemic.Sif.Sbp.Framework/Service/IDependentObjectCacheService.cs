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
using Systemic.Sif.Sbp.Framework.Model;

namespace Systemic.Sif.Sbp.Framework.Service
{

    /// <summary>
    /// This interface specifies operations used by the Dependent Object Cache (DOC).
    /// </summary>
    public interface IDependentObjectCacheService
    {

        /// <summary>
        /// Delete the CachedObject from the cache if it exists. If not, then this method will do nothing.
        /// </summary>
        /// <param name="cachedObject">The CachedObject to delete.</param>
        /// <exception cref="System.ArgumentNullException">cachedObject parameter is null.</exception>
        void DeleteCachedObject(CachedObject cachedObject);

        /// <summary>
        /// Delete the DependentObject from the cache if it exists. If not, then this method will do nothing.
        /// </summary>
        /// <param name="cachedObject">The DependentObject to delete.</param>
        /// <exception cref="System.ArgumentNullException">cachedObject parameter is null.</exception>
        void DeleteDependentObject(DependentObject dependentObject);

        /// <summary>
        /// Mark the DependentObject in the cache has already requested.
        /// </summary>
        /// <param name="dependentObject">The DependentObject to mark.</param>
        /// <param name="agentId">Agent associated with the dependent object.</param>
        /// <param name="zoneId">Zone associated with the dependent object.</param>
        void MarkAsRequested(DependentObject dependentObject, string agentId, string zoneId);

        /// <summary>
        /// Retrieve cached objects that have no remaining dependencies.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type for the cached object.</param>
        /// <param name="applicationId">Application associated with the cached object.</param>
        /// <param name="agentId">Agent associated with the cached object.</param>
        /// <returns>Cached objects with no dependencies if they exist; empty collection otherwise.</returns>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        ICollection<CachedObject> RetrieveByNoDependencies(string sifObjectName, string applicationId, string agentId);

        /// <summary>
        /// Retrieve a CachedObject from the cache based upon the SIF data object/message, application and zone.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type for the cached object.</param>
        /// <param name="objectKeyValue">Key value associated with the cached object.</param>
        /// <param name="applicationId">Application associated with the SIF data object/message.</param>
        /// <param name="zoneId">Zone associated with the SIF data object/message.</param>
        /// <returns>Requested CachedObject from the cache if it exists; null otherwise.</returns>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        CachedObject RetrieveCachedObject(string sifObjectName, string flatKey, string applicationId, string zoneId);

        /// <summary>
        /// Retrieve a dependent object instance based upon the passed parameters.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type for the dependent object.</param>
        /// <param name="objectKeyValue">Key value associated with the dependent object.</param>
        /// <param name="applicationId">Application associated with the dependent object.</param>
        /// <param name="zoneId">Zone associated with the dependent object.</param>
        /// <returns>A dependent object instance based upon the passed parameters if found; null otherwise.</returns>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        /// <exception cref="Systemic.Sif.Sbp.Framework.Persistence.PersistenceException">Duplicate dependent object entries exist.</exception>
        DependentObject RetrieveDependentObject(string sifObjectName, string objectKeyValue, string applicationId, string zoneId);

        /// <summary>
        /// Retrieve dependent objects that have not had a request made for them.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type for the dependent object.</param>
        /// <param name="applicationId">Application associated with the dependent object.</param>
        /// <param name="zoneId">Zone associated with the dependent object.</param>
        /// <returns>Dependent objects not yet requested; empty collection if there are none.</returns>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        ICollection<DependentObject> RetrieveNotYetRequested(string sifObjectName, string applicationId, string zoneId);

        /// <summary>
        /// Store the SIF data object/message in the cache.
        /// If no dependentObjects are associated with the SIF data object/message (null or empty), then it will not
        /// be stored.
        /// If eventAction is null, then it is assumed that the SIF data object/message was received in
        /// Request/Response mode.
        /// </summary>
        /// <param name="sifDataObject">SIF data object/message.</param>
        /// <param name="eventAction">EventAction associated with Event mode.</param>
        /// <param name="agentId">Agent associated with the SIF data object/message.</param>
        /// <param name="applicationId">Application associated with the SIF data object/message.</param>
        /// <param name="zoneId">Zone associated with the SIF data object/message.</param>
        /// <param name="dependentObjects">Dependent objects associated with the SIF data object/message.</param>
        /// <exception cref="System.ArgumentException">agentId, applicationId and/or zoneId parameter is null or empty.</exception>
        /// <exception cref="System.ArgumentNullException">sifDataObject parameter is null.</exception>
        /// <exception cref="System.InvalidOperationException">Unable to find metadata for the SIF data object/message.</exception>
        void StoreObjectInCache
            (string sifObjectName,
            string objectKeyValue,
            string sifDataObjectXml,
            string eventAction,
            string agentId,
            string applicationId,
            string zoneId,
            string expiryStrategy,
            int expiryPeriod,
            ICollection<DependentObject> dependentObjects);

        /// <summary>
        /// Update all cached objects, and their dependents, according to it's expiry strategy. Currently 2 strategies
        /// are supported: EXPIRE and REQUEST. In the case of EXPIRE, the cached object and it's dependents will be
        /// removed from the cache. In the case of REQUEST, all dependent objects for each cached object will be marked
        /// as unrequested so that they can be requested again by the appropriate Subscribers. The cached object will
        /// also update it's expiry date accordingly.
        /// </summary>
        /// <param name="applicationId">Application associated with the cached object.</param>
        /// <param name="agentId">Agent associated with the cached object.</param>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        void UpdateExpiredObjects(string applicationId, string agentId, string expiryStrategy, int expiryPeriod);

    }

}
