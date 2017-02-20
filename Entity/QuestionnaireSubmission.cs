using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class QuestionnaireSubmission : IEntity, IEntityWithTimeStamp, IEntityWithUserStamp, IEntityWithRestrictedAccess
    {
        [Key]
        public int ID { get; set; }
        [Index]
        [Required]
        [MaxLength(50)]
        public string CodeName { get; set; }
        [Required]
        public string CreatedByApplicationUserId { get; set; }
        [Required]
        public string UpdatedByApplicationUserId { get; set; }
        [Required]
        public int QuestionnaireID { get; set; }
        [Required]
        public string SubmissionXml { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }

        #region Virtual properties

        [ForeignKey("QuestionnaireID")]
        public Questionnaire Questionnaire { get; set; }

        [ForeignKey("CreatedByApplicationUserId")]
        public ApplicationUser CreatedByApplicationUser { get; set; }

        [ForeignKey("UpdatedByApplicationUserId")]
        public ApplicationUser UpdatedByApplicationUser { get; set; }

        #endregion

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(GetObjectID().ToString());
        }

        #endregion
    }
}
