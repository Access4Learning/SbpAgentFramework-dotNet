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

using Systemic.Sif.Sbp.Framework.Model;
using System.Collections.Generic;

namespace Systemic.Sif.Sbp.Framework.Persistence
{

    public interface IDependentObjectDao : IGenericDao<DependentObject>
    {

        void DeleteOrphans();

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
        DependentObject Retrieve(string sifObjectName, string objectKeyValue, string applicationId, string zoneId);

        /// <summary>
        /// Retrieve dependent objects (based upon the passed parameters) that have not had a request made for them.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type for the dependent object.</param>
        /// <param name="applicationId">Application associated with the dependent object.</param>
        /// <param name="zoneId">Zone associated with the dependent object.</param>
        /// <returns>Dependent objects not yet requested (if they exist); empty collection otherwise.</returns>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        ICollection<DependentObject> RetrieveNotYetRequested(string sifObjectName, string applicationId, string zoneId);

    }

}
