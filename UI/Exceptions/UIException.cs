using System;
using System.Runtime.Serialization;

namespace UI.Exceptions
{
    public class UIException : Exception
    {
        /// <summary>
        /// Just create the exception
        /// </summary>
        public UIException()
            : base()
        {
        }

        /// <summary>
        /// Create the exception using given type
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        public UIException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create the exception using given type
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="innerException">Inner exception</param>
        public UIException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Create the exception using given type
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        public UIException(UIExceptionEnum exceptionType)
            : base(UIExceptionHelper.GetExceptionMessage(exceptionType))
        {
        }

        /// <summary>
        /// Create the exception using given type
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="innerException">Exception inner cause</param>
        public UIException(UIExceptionEnum exceptionType, Exception innerException)
            : base(UIExceptionHelper.GetExceptionMessage(exceptionType), innerException)
        {
        }

        /// <summary>
        /// Create the exception from serialized data.
        /// Usual scenario is when exception is occured somewhere on the remote workstation
        /// and we have to re-create/re-throw the exception on the local machine
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Serialization context</param>
        protected UIException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
