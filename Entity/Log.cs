using System;
using Entity.Base;

namespace Entity
{
    public class Log : IEntity
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public string Url { get; set; }
        public string ExceptionMessage { get; set; }
        public string InnerException { get; set; }
        public string Stacktrace { get; set; }
        public string ApplicationUserName { get; set; }

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return ID.ToString();
        }

        #endregion
    }
}
