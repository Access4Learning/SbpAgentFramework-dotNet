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

using System;
using System.Collections.Generic;
using Systemic.Sif.Sbp.Framework.Model;
using Systemic.Sif.Sbp.Framework.Persistence;
using Systemic.Sif.Sbp.Framework.Persistence.NHibernate;

namespace Systemic.Sif.Sbp.Framework.Service
{

    public class DependentObjectCacheService : IDependentObjectCacheService
    {
        // Create a logger for use in this class.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ICachedObjectDao cachedObjectDao = new CachedObjectDao();
        IDependentObjectDao dependentObjectDao = new DependentObjectDao();

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.IDependentObjectCacheService.DeleteCachedObject(CachedObject)">DeleteCachedObject</see>
        public void DeleteCachedObject(CachedObject cachedObject)
        {
            cachedObjectDao.Delete(cachedObject);
        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.IDependentObjectCacheService.DeleteDependentObject(DependentObject)">DeleteDependentObject</see>
        public void DeleteDependentObject(DependentObject dependentObject)
        {
            dependentObjectDao.Delete(dependentObject);
        }

        public void MarkAsRequested(DependentObject dependentObject, string agentId, string zoneId)
        {

            if (dependentObject == null)
            {
                throw new ArgumentNullException("cachedObject");
            }

            if (String.IsNullOrEmpty(agentId) || String.IsNullOrEmpty(zoneId))
            {
                throw new ArgumentException("agentId and/or zoneId parameter is null or empty.");
            }

            dependentObject.AgentId = agentId;
            dependentObject.ZoneId = zoneId;
            dependentObject.RequestedDate = DateTime.Now;
            dependentObject.Requested = true;
            dependentObjectDao.Save(dependentObject);
        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.IDependentObjectCacheService.RetrieveByNoDependencies(string, string, string)">RetrieveByNoDependencies</see>
        public ICollection<CachedObject> RetrieveByNoDependencies(string sifObjectName, string applicationId, string agentId)
        {
            return cachedObjectDao.RetrieveByNoDependencies(sifObjectName, applicationId, agentId);
        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.IDependentObjectCacheService.RetrieveCachedObject(string, string, string, string)">RetrieveCachedObject</see>
        public CachedObject RetrieveCachedObject(string sifObjectName, string flatKey, string applicationId, string zoneId)
        {
            return cachedObjectDao.Retrieve(sifObjectName, flatKey, applicationId, zoneId);
        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.IDependentObjectCacheService.RetrieveDependentObject(string, string, string, string)">RetrieveDependentObject</see>
        public DependentObject RetrieveDependentObject(string sifObjectName, string objectKeyValue, string applicationId, string zoneId)
        {
            return dependentObjectDao.Retrieve(sifObjectName, objectKeyValue, applicationId, zoneId);
        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.IDependentObjectCacheService.RetrieveNotYetRequested(string, string, string)">RetrieveNotYetRequested</see>
        public ICollection<DependentObject> RetrieveNotYetRequested(string sifObjectName, string applicationId, string zoneId)
        {
            return dependentObjectDao.RetrieveNotYetRequested(sifObjectName, applicationId, zoneId);
        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.IDependentObjectCacheService.StoreObjectInCache((SifDataObject, EventAction, string, string, string)">StoreObjectInCache</see>
        public void StoreObjectInCache
            (string sifObjectName,
            string objectKeyValue,
            string sifDataObjectXml,
            string eventAction,
            string agentId,
            string applicationId,
            string zoneId,
            string expiryStrategy,
            int expiryPeriod,
            ICollection<DependentObject> dependentObjects)
        {

            if (String.IsNullOrEmpty(sifObjectName) ||
                String.IsNullOrEmpty(objectKeyValue) ||
                String.IsNullOrEmpty(sifDataObjectXml) ||
                String.IsNullOrEmpty(agentId) ||
                String.IsNullOrEmpty(applicationId) ||
                String.IsNullOrEmpty(zoneId) ||
                String.IsNullOrEmpty(expiryStrategy))
            {
                throw new ArgumentException("A parameter is null or empty.");
            }

    	    if (dependentObjects == null || dependentObjects.Count == 0)
    	    {
                if (log.IsInfoEnabled) log.Info("The following SIF Data Object has no dependent objects and will therefore not be cached:\n" + sifDataObjectXml);
    	    }
    	    else
    	    {
                CachedObject cachedObject = new CachedObject();
                cachedObject.SifObjectName = sifObjectName;
                cachedObject.ObjectKeyValue = objectKeyValue;
                cachedObject.ObjectXml = sifDataObjectXml;
                cachedObject.ReceivedDate = DateTime.Now;
                cachedObject.AgentId = agentId;
                cachedObject.ApplicationId = applicationId;
                cachedObject.ZoneId = zoneId;
                cachedObject.RemainingDependencies = dependentObjects.Count;
                cachedObject.ExpiryDate = cachedObject.ReceivedDate.AddMilliseconds(expiryPeriod);
                cachedObject.ExpiryStrategy = expiryStrategy;
                cachedObject.DependentObjects = dependentObjects;

                if (eventAction == null)
                {
                    cachedObject.IsEvent = false;
                }
                else
                {
                    cachedObject.IsEvent = true;
                    cachedObject.EventType = eventAction;
                }

                // Iterate through all dependent objects and set the application and zone IDs.
	    	    foreach (DependentObject dependentObject in cachedObject.DependentObjects)
	    	    {

                    // Ensure that the dependent object has not yet been requested.
	    		    if (!dependentObject.Requested)
	    		    {
	    			    // Don't assign the agent because we don't know which agent is responsible for requesting it. Instead
	    			    // assign the application ID because whichever agent is responsible for the retrieval of this dependent
	    			    // object must pick it up for that application.
	    			    dependentObject.ZoneId = zoneId;
	    			    dependentObject.Requested = false;
	    			    dependentObject.ApplicationId = applicationId;
	    		    }

                }

                cachedObjectDao.Save(cachedObject);
    	    }

        }

        /// <see cref="Systemic.Sif.Sbp.Framework.Service.IDependentObjectCacheService.UpdateExpiredObjects(string, string, string, int)">UpdateExpiredObjects</see>
        public void UpdateExpiredObjects(string applicationId, string agentId, string expiryStrategy, int expiryPeriod)
        {

            if (String.IsNullOrEmpty(applicationId) || String.IsNullOrEmpty(agentId))
            {
                throw new ArgumentException("A string parameter is null or empty.");
            }

            ICollection<CachedObject> expiredObjects = cachedObjectDao.RetrieveExpiredObjects(applicationId, agentId);

            foreach (CachedObject expiredObject in expiredObjects)
            {

                if ("EXPIRE".Equals(expiredObject.ExpiryStrategy))
                {
                    cachedObjectDao.Delete(expiredObject);
                    if (log.IsDebugEnabled) log.Debug(expiredObject.SifObjectName + " (" + expiredObject.ObjectKeyValue + ") has been expired and removed from the cached.");
                }
                else if ("REQUEST".Equals(expiredObject.ExpiryStrategy))
                {

                    foreach (DependentObject dependentObject in expiredObject.DependentObjects)
                    {
                        dependentObject.Requested = false;
                    }

                    expiredObject.ExpiryDate.AddMinutes(expiryPeriod);
                    if (log.IsDebugEnabled) log.Debug(expiredObject.SifObjectName + " (" + expiredObject.ObjectKeyValue + ") has expired and it's dependents will be requested again after an expiry period of " + expiryPeriod + ".");

                    if (!String.IsNullOrEmpty(expiryStrategy) && !expiryStrategy.Equals(expiredObject.ExpiryStrategy))
                    {
                        if (log.IsDebugEnabled) log.Debug(expiredObject.SifObjectName + " (" + expiredObject.ObjectKeyValue + ") will have it's expiry strategy changed from " + expiredObject.ExpiryStrategy + " to " + expiryStrategy + ".");
                        expiredObject.ExpiryStrategy = expiryStrategy;
                    }

                    cachedObjectDao.Save(expiredObject);
                }

            }

        }

    }

}
