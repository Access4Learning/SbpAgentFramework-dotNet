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

using Systemic.Sif.Sbp.Framework.Model;

namespace Systemic.Sif.Sbp.Framework.Persistence
{

    public interface IObjectZoneSyncDao : IGenericDao<ObjectZoneSync>
    {

        /// <summary>
        /// Retrieve an ObjectZoneSync instance based upon the passed parameters.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type.</param>
        /// <param name="agentId">ID of the Agent.</param>
        /// <param name="zoneId">ID of the Zone.</param>
        /// <returns>An ObjectZoneSync instance based upon the passed parameters if found; null otherwise.</returns>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        /// <exception cref="Systemic.Sif.Sbp.Framework.Persistence.PersistenceException">Duplicate object/zone sync entries exist.</exception>
        ObjectZoneSync Retrieve(string sifObjectName, string agentId, string zoneId);

    }

}
