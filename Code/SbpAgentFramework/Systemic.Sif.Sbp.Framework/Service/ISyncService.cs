
namespace Systemic.Sif.Sbp.Framework.Service
{

    public interface ISyncService
    {

        /// <summary>
        /// Update the last requested date for an object/zone sync entry. If an entry does not exist, create one.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type.</param>
        /// <param name="agentId">ID of the Agent.</param>
        /// <param name="zoneId">ID of the Zone.</param>
        /// <exception cref="System.ArgumentNullException">A parameter is null or empty.</exception>
        /// <exception cref="Systemic.Sif.Sbp.Framework.Persistence.PersistenceException">Duplicate object/zone sync entries exist.</exception>
        void MarkAsSynced(string sifObjectName, string agentId, string zoneId);

        /// <summary>
        /// Determine whether a sync is required for the object/zone combination.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type.</param>
        /// <param name="agentId">ID of the Agent.</param>
        /// <param name="zoneId">ID of the Zone.</param>
        /// <returns>True if a sync is required (an object/zone sync entry does NOT exist); false otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">A parameter is null or empty.</exception>
        /// <exception cref="Systemic.Sif.Sbp.Framework.Persistence.PersistenceException">Duplicate object/zone sync entries exist.</exception>
        bool IsSyncRequired(string sifObjectName, string agentId, string zoneId);

    }

}
