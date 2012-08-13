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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenADK.Library.au.Infrastructure;

namespace Systemic.Sif.Sbp.Framework.Model.Metadata
{

    /// <summary>
    /// Framework meta-data associated with the Identity SIF Object.
    /// </summary>
    public class IdentityMetadata : SifDataObjectMetadata<Identity>
    {

        // Dependent objects associated with the Identity object instance.
        public override ICollection<DependentObject> DependentObjects
        {

            get
            {

                return new Collection<DependentObject>()
                {
                    new DependentObject() { SifObjectName = sifDataObject.SIF_RefId.SIF_RefObject, ObjectKeyValue = "@RefId=" + sifDataObject.SIF_RefId }
                };

            }

        }

        /// <summary>
        /// Create an instance of an IdentityMetadata.
        /// </summary>
        /// <param name="identity">Identity object of interest.</param>
        public IdentityMetadata(Identity identity)
            : base(identity)
        {
        }

    }

}
