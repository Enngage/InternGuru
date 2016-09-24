using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Message : EntityAbstract
    {
        public int ID { get; set; }
        [ForeignKey("Company")]
        public int RecipientCompanyID { get; set; }
        [ForeignKey("SenderApplicationUser")]
        public string SenderApplicationUserId { get; set; }
        [ForeignKey("RecipientApplicationUser")]
        public string RecipientApplicationUserId { get; set; }
        public DateTime MessageCreated { get; set; }
        public string MessageText { get; set; }
        public bool IsRead { get; set; }
        [MaxLength(200)]
        public string Subject { get; set; }

        #region Virtual properties

        public Company Company { get; set; }
        public ApplicationUser SenderApplicationUser { get; set; }
        public ApplicationUser RecipientApplicationUser { get; set; }

        #endregion

        #region EntityAbstract members

        public override object GetObjectID()
        {
            return ID;
        }
        public override string GetCodeName()
        {
            return ID.ToString();
        }

        #endregion

    }
}
