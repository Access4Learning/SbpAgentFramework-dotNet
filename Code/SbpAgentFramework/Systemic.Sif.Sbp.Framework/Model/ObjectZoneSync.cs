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
