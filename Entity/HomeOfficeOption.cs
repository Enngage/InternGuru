
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class HomeOfficeOption : IEntity, IEntityWithUniqueCodeName
    {
        public int ID { get; set; }
        [MaxLength(50)]
        [Required]
        public string HomeOfficeName { get; set; }
        [Index]
        [Required]
        [MaxLength(50)]
        public string CodeName { get; set; }
        [MaxLength(50)]
        public string IconClass { get; set; }

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(HomeOfficeName);
        }

        #endregion
    }
}
