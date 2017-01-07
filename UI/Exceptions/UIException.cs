using System;
using System.Runtime.Serialization;

namespace UI.Exceptions
{
    public class UiException : Exception
    {
        /// <summary>
        /// Just create the exception
        /// </summary>
        public UiException()
        {
        }

        /// <summary>
        /// Create the exception using given type
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        public UiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create the exception using given type
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="innerException">Inner exception</param>
        public UiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Create the exception using given type
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        public UiException(UiExceptionEnum exceptionType)
            : base(UiExceptionHelper.GetExceptionMessage(exceptionType))
        {
        }

        /// <summary>
        /// Create the exception using given type
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="innerException">Exception inner cause</param>
        public UiException(UiExceptionEnum exceptionType, Exception innerException)
            : base(UiExceptionHelper.GetExceptionMessage(exceptionType), innerException)
        {
        }

        /// <summary>
        /// Create the exception from serialized data.
        /// Usual scenario is when exception is occured somewhere on the remote workstation
        /// and we have to re-create/re-throw the exception on the local machine
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Serialization context</param>
        protected UiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
