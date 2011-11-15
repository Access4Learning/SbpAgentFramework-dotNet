using System;
using System.Runtime.Serialization;

namespace Systemic.Sif.Sbp.Framework.Persistence
{

    public class PersistenceException : Exception
    {

        /// <summary>
        /// Initialises a new instance of this class.
        /// </summary>
        public PersistenceException()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of this class with the specified error message.
        /// </summary>
        /// <param name="message">The message to display for this exception.</param>
        public PersistenceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of this class with the specified error message and a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        public PersistenceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with serialized data. This constructor is needed for serialization.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination. </param>
        protected PersistenceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }

}
