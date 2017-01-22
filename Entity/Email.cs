
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity
{
    public class Email : IEntity
    {
        public int ID { get; set; }
        [Index]
        public Guid Guid { get; set; }
        [MaxLength(100)]
        public string Subject { get; set; }
        [MaxLength(200)]
        public string To { get; set; }
        [MaxLength(200)]
        public string From { get; set; }
        public string HtmlBody { get; set; }
        public bool IsSent { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Sent { get; set; }
        public string Result { get; set; }

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return GetObjectID().ToString();
        }

        #endregion
    }
}
