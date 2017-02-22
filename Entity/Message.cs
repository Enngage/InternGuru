using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class Message : IEntity, IEntityWithTimeStamp
    {
        public int ID { get; set; }
        [Index]
        [MaxLength(100)]
        [Required]
        public string CodeName { get; set; }
        [Required]
        public string SenderApplicationUserId { get; set; }
        [Required]
        public string RecipientApplicationUserId { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
        [Required]
        public string MessageText { get; set; }
        [Required]
        public bool IsRead { get; set; }
        [MaxLength(200)]
        public string Subject { get; set; }
        public int? QuestionnaireSubmissionID { get; set; }

        #region Virtual properties
        [ForeignKey("QuestionnaireSubmissionID")]
        public QuestionnaireSubmission QuestionnaireSubmission { get; set; }
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
