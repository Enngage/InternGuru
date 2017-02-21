
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class Email : IEntity, IEntityWithTimeStamp, IEntityWithGuid, IEntityWithOptionalUserStamp
    {
        public int ID { get; set; }
        [Index]
        [Required]
        public Guid Guid { get; set; }
        [Index]
        [Required]
        [MaxLength(100)]
        public string CodeName { get; set; }
        [MaxLength(100)]
        public string Subject { get; set; }
        [MaxLength(200)]
        public string To { get; set; }
        [MaxLength(200)]
        public string From { get; set; }
        public string HtmlBody { get; set; }
        public bool IsSent { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? Sent { get; set; }
        public string Result { get; set; }
        [Required]
        public string CreatedByApplicationUserId { get; set; }
        [Required]
        public string UpdatedByApplicationUserId { get; set; }

        #region IEntity members

        [ForeignKey("CreatedByApplicationUserId")]
        public ApplicationUser CreatedByApplicationUser { get; set; }

        [ForeignKey("UpdatedByApplicationUserId")]
        public ApplicationUser UpdatedByApplicationUser { get; set; }

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(ID.ToString());
        }

        #endregion
    }
}
