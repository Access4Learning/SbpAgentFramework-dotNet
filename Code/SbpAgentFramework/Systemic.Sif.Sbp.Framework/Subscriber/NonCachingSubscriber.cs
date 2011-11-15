using Edustructures.SifWorks;
using Edustructures.SifWorks.Tools.Cfg;

namespace Systemic.Sif.Sbp.Framework.Subscriber
{

    /// <summary>
    /// Subscribers of SIF Data Objects that are NOT defined as part of the SIF Baseline Profile (SBP) must extend this
    /// class rather than CachingSubscriber. This class still supports the functionality of controlling start-up
    /// sequencing and management through a database (much like the CachingSubscriber), but does not have the Dependent
    /// Object Cache (DOC) behind the scenes. This means objects received by this Subscriber will be processed
    /// immediately rather then checking for dependencies.
    /// Subscribers of SIF Data Objects that are defined as part of the SIF Baseline Profile (SBP) may also extend this
    /// class if they do not require the DOC.
    /// </summary>
    /// <typeparam name="T">The type of SIF Data Object this Subscriber processes, e.g. StudentPersonal, Schoolnfo, etc.</typeparam>
    public abstract class NonCachingSubscriber<T> : SyncSubscriber<T> where T : SifDataObject, new()
    {

        public NonCachingSubscriber()
            : base()
        {
        }

        public NonCachingSubscriber(AgentConfig agentConfig)
            : base(agentConfig)
        {
        }

    }

}
