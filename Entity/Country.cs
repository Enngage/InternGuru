
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class Country : IEntity, IEntityWithUniqueCodeName
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        [Required]
        public string CountryCode { get; set; }
        [MaxLength(50)]
        [Required]
        public string CountryName { get; set; }
        [Index]
        [Required]
        [MaxLength(50)]
        public string CodeName { get; set; }
        public string Icon { get; set; }

        #region Virtual properties
        [ForeignKey("CountryID")] // Internship model needs to point here
        public ICollection<Internship> Internships { get; set; }
        [ForeignKey("CountryID")] // Companies model needs to point here model needs to point here
        public ICollection<Company> Companies { get; set; }

        #endregion

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(CountryCode);
        }

        #endregion
    }
}
