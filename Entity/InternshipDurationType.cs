
using Core.Helpers;
using Core.Helpers.Internship;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity
{
    public class InternshipDurationType : IEntity
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string DurationName { get; set; }
        [Index]
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
            return CodeName;
        }

        #endregion
    }
}
