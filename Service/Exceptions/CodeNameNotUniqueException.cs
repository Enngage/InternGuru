using System;
using System.Runtime.Serialization;

namespace Service.Exceptions
{
    public class CodeNameNotUniqueException : Exception
    {
        /// <summary>
        /// Just create the exception
        /// </summary>
        public CodeNameNotUniqueException()
        {
        }

        /// <summary>
        /// Create the exception with description
        /// </summary>
        /// <param name="message">Exception description</param>
        public CodeNameNotUniqueException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create the exception with description and inner cause
        /// </summary>
        /// <param name="message">Exception description</param>
        /// <param name="innerException">Exception inner cause</param>
        public CodeNameNotUniqueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Create the exception from serialized data.
        /// Usual scenario is when exception is occured somewhere on the remote workstation
        /// and we have to re-create/re-throw the exception on the local machine
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Serialization context</param>
        protected CodeNameNotUniqueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
