using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class Questionare : IEntity
    {
        [Key]
        public int ID { get; set; }
        [Index]
        [Required]
        [MaxLength(50)]
        public string CodeName { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public int CompanyID { get; set; }
        [Required]
        public string QuestionareName { get; set; }
        [Required]
        public string QuestionareDefinitionXml { get; set; }

        #region Virtual properties

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("CompanyID")]
        public Company Company { get; set; }

        #endregion

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(QuestionareName);
        }

        #endregion
    }
}
