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

using System;
using System.Collections.Generic;
using System.Text;
using Systemic.Sif.Sbp.Framework.Persistence;
using System.Collections.ObjectModel;

namespace Systemic.Sif.Sbp.Framework.Model
{

    public class DependentObject : IPersistable
    {

        public virtual long Id { get; set; }

        public virtual string SifObjectName { get; set; }

        public virtual string ObjectKeyValue { get; set; }

        public virtual string AgentId { get; set; }

        public virtual string ApplicationId { get; set; }

        public virtual bool Requested { get; set; }

        public virtual DateTime RequestedDate { get; set; }

        public virtual string ZoneId { get; set; }

        public virtual ICollection<CachedObject> ParentObjects { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<DependentObject>");
            stringBuilder.Append("[");
            stringBuilder.Append("Id=").Append(Id).Append("|");
            stringBuilder.Append("SifObjectName=").Append(SifObjectName).Append("|");
            stringBuilder.Append("ObjectKeyValue=").Append(ObjectKeyValue).Append("|");
            stringBuilder.Append("AgentId=").Append(AgentId).Append("|");
            stringBuilder.Append("ApplicationId=").Append(ApplicationId).Append("|");
            stringBuilder.Append("Requested=").Append(Requested).Append("|");
            stringBuilder.Append("RequestedDate=").Append(RequestedDate).Append("|");
            stringBuilder.Append("ZoneId=").Append(ZoneId).Append("|");

            if (ParentObjects == null)
            {
                stringBuilder.Append("ParentObjects=NULL");
            }
            else if (!ParentObjects.GetType().Equals(typeof(Collection<CachedObject>)))
            {
                stringBuilder.Append("ParentObjects=PROXY");
            }
            else if (ParentObjects.Count == 0)
            {
                stringBuilder.Append("ParentObjects=EMPTY");
            }
            else
            {

                foreach (CachedObject parentObject in ParentObjects)
                {
                    stringBuilder.Append("\n    [");
                    stringBuilder.Append("ParentObject=").Append(parentObject);
                    stringBuilder.Append("]");
                }

            }

            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

    }

}
