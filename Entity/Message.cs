using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class Message : IEntity
    {
        public int ID { get; set; }
        [Index]
        [MaxLength(100)]
        public string CodeName { get; set; }
        public int RecipientCompanyID { get; set; }
        public string SenderApplicationUserId { get; set; }
        public string RecipientApplicationUserId { get; set; }
        public DateTime MessageCreated { get; set; }
        public string MessageText { get; set; }
        public bool IsRead { get; set; }
        [MaxLength(200)]
        public string Subject { get; set; }

        #region Virtual properties

        [ForeignKey("RecipientCompanyID")]
        public Company Company { get; set; }
        [ForeignKey("SenderApplicationUserId")]
        public ApplicationUser SenderApplicationUser { get; set; }
        [ForeignKey("RecipientApplicationUserId")]
        public ApplicationUser RecipientApplicationUser { get; set; }

        #endregion

        #region IEntity members

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
