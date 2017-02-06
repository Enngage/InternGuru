
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class CompanySize : IEntity, IEntityWithUniqueCodeName
    {
        public int ID { get; set; }
        [MaxLength(100)]
        [Required]
        public string CompanySizeName { get; set; }
        [Index]
        [Required]
        [MaxLength(100)]
        public string CodeName { get; set; }

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(CompanySizeName);
        }

        #endregion
    }
}
