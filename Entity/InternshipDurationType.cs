
using Core.Helpers;
using Core.Helpers.Internship;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity
{
    public class InternshipDurationType : IEntity, IEntityWithUniqueCodeName
    {
        public int ID { get; set; }
        [MaxLength(50)]
        [Required]
        public string DurationName { get; set; }
        [Index]
        [Required]
        [MaxLength(50)]
        public string CodeName { get; set; }

        #region Not mapped properties

        [NotMapped]
        public virtual InternshipDurationTypeEnum DurationTypeEnum
        {
            get
            {
                return EnumHelper.ParseEnum<InternshipDurationTypeEnum>(CodeName);
            }
        }

        #endregion

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(DurationName);
        }

        #endregion
    }
}
