using Edustructures.SifWorks;
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
