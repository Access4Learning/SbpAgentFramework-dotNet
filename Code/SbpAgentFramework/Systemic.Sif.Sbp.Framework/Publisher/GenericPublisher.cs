using Edustructures.SifWorks;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Publisher
{

    public abstract class GenericPublisher<T> : Systemic.Sif.Framework.Publisher.GenericPublisher<T> where T : SifDataObject, new()
    {

        public GenericPublisher()
            : base()
        {
        }

        public GenericPublisher(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
