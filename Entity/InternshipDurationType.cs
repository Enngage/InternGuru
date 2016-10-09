
using Common.Helpers;
using Common.Helpers.Internship;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class InternshipDurationType : IEntity
    {
        public int ID { get; set; }
        public string DurationName { get; set; }
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
