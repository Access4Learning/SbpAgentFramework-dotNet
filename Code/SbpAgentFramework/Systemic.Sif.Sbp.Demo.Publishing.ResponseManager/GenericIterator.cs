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
using Systemic.Sif.Framework.Model;
using Systemic.Sif.Framework.Publisher;

namespace Systemic.Sif.Sbp.Demo.Publishing.ResponseManager
{

    abstract class GenericIterator<T> : ISifEventIterator<T>, ISifResponseIterator<T> where T : SifDataObject, new()
    {

        public virtual void AfterEvent()
        {
        }

        public virtual void BeforeEvent()
        {
        }

        public abstract SifEvent<T> GetNextEvent();

        public abstract bool HasNextEvent();

        public virtual void AfterResponse()
        {
        }

        public virtual void BeforeResponse()
        {
        }

        public abstract T GetNextResponse();

        public abstract bool HasNextResponse();

    }

}
