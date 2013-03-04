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
using System.Text;
using Systemic.Sif.Sbp.Framework.Persistence;

namespace Systemic.Sif.Sbp.Framework.Model
{

    public class ObjectZoneSync : IPersistable
    {

        public virtual long Id { get; set; }

        public virtual string SifObjectName { get; set; }

        public virtual string ZoneId { get; set; }

        public virtual string AgentId { get; set; }

        public virtual DateTime LastRequested { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<ObjectZoneSync>");
            stringBuilder.Append("[");
            stringBuilder.Append("Id=").Append(Id).Append("|");
            stringBuilder.Append("SifObjectName=").Append(SifObjectName).Append("|");
            stringBuilder.Append("ZoneId=").Append(ZoneId).Append("|");
            stringBuilder.Append("AgentId=").Append(AgentId).Append("|");
            stringBuilder.Append("LastRequested=").Append(LastRequested);
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

    }

}
