using Systemic.Sif.Sbp.Framework.Model;

namespace Systemic.Sif.Sbp.Framework.Persistence
{

    public interface IObjectZoneSyncDao : IGenericDao<ObjectZoneSync>
    {

        /// <summary>
        /// Retrieve an ObjectZoneSync instance based upon the passed parameters.
        /// </summary>
        /// <param name="sifObjectName">Name of the SIF Object type.</param>
        /// <param name="agentId">ID of the Agent.</param>
        /// <param name="zoneId">ID of the Zone.</param>
        /// <returns>An ObjectZoneSync instance based upon the passed parameters if found; null otherwise.</returns>
        /// <exception cref="System.ArgumentException">A parameter is null or empty.</exception>
        /// <exception cref="Systemic.Sif.Sbp.Framework.Persistence.PersistenceException">Duplicate object/zone sync entries exist.</exception>
        ObjectZoneSync Retrieve(string sifObjectName, string agentId, string zoneId);

    }

}
