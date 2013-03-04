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
using System.Collections.ObjectModel;
using System.Text;
using Systemic.Sif.Sbp.Framework.Persistence;

namespace Systemic.Sif.Sbp.Framework.Model
{

    public class CachedObject : IPersistable
    {

        public virtual long Id { get; set; }

        public virtual string SifObjectName { get; set; }

        public virtual string ObjectKeyValue { get; set; }

        public virtual string ObjectXml { get; set; }

        public virtual bool IsEvent { get; set; }

        public virtual string EventType { get; set; }

        public virtual DateTime ReceivedDate { get; set; }

        public virtual string AgentId { get; set; }

        public virtual string ApplicationId { get; set; }

        public virtual string ZoneId { get; set; }

        public virtual int RemainingDependencies { get; set; }

        public virtual DateTime ExpiryDate { get; set; }

        public virtual string ExpiryStrategy { get; set; }

        public virtual ICollection<DependentObject> DependentObjects { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<CachedObject>");
            stringBuilder.Append("[");
            stringBuilder.Append("Id=").Append(Id).Append("|");
            stringBuilder.Append("SifObjectName=").Append(SifObjectName).Append("|");
            stringBuilder.Append("ObjectKeyValue=").Append(ObjectKeyValue).Append("|");
            stringBuilder.Append("ObjectXml=").Append(ObjectXml).Append("|");
            stringBuilder.Append("IsEvent=").Append(IsEvent).Append("|");
            stringBuilder.Append("EventType=").Append(EventType).Append("|");
            stringBuilder.Append("ReceivedDate=").Append(ReceivedDate).Append("|");
            stringBuilder.Append("AgentId=").Append(AgentId).Append("|");
            stringBuilder.Append("ApplicationId=").Append(ApplicationId).Append("|");
            stringBuilder.Append("ZoneId=").Append(ZoneId).Append("|");
            stringBuilder.Append("RemainingDependencies=").Append(RemainingDependencies).Append("|");
            stringBuilder.Append("ExpiryDate=").Append(ExpiryDate).Append("|");
            stringBuilder.Append("ExpiryStrategy=").Append(ExpiryStrategy).Append("|");

            if (DependentObjects == null)
            {
                stringBuilder.Append("DependentObjects=NULL");
            }
            else if (!DependentObjects.GetType().Equals(typeof(Collection<DependentObject>)))
            {
                stringBuilder.Append("DependentObjects=PROXY");
            }
            else if (DependentObjects.Count == 0)
            {
                stringBuilder.Append("DependentObjects=EMPTY");
            }
            else
            {

                foreach (DependentObject dependentObject in DependentObjects)
                {
                    stringBuilder.Append("\n    [");
                    stringBuilder.Append("DependentObject=").Append(dependentObject);
                    stringBuilder.Append("]");
                }

            }

            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

    }

}
