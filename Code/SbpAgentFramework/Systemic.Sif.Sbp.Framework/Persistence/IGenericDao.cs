using System.Collections.Generic;

namespace Systemic.Sif.Sbp.Framework.Persistence
{

    public interface IGenericDao<T> where T : IPersistable
    {

        /// <summary>
        /// Save the object.
        /// </summary>
        /// <param name="obj">Object to save.</param>
        /// <exception cref="System.ArgumentNullException">obj parameter is null.</exception>
        void Save(T obj);

        /// <summary>
        /// Delete the object.
        /// </summary>
        /// <param name="obj">Object to delete.</param>
        /// <exception cref="System.ArgumentNullException">obj parameter is null.</exception>
        void Delete(T obj);

        /// <summary>
        /// Retrieve the object based upon it's unique identifier.
        /// </summary>
        /// <param name="objId">Unique identifier for the object.</param>
        /// <returns>Object defined by the passed unique identifier.</returns>
        T RetrieveById(long objId);

        /// <summary>
        /// Retrieve all objects.
        /// </summary>
        /// <returns>All objects.</returns>
        ICollection<T> RetrieveAll();

    }

}
