using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class InternshipCategory : IEntity, IEntityWithUniqueCodeName
    {

        #region DB 

        public int ID { get; set; }
        [Index]
        [Required]
        [MaxLength(50)]
        public string CodeName { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        #endregion

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(Name);
        }

        #endregion

        #region Virtual properties

        public ICollection<Internship> Internships { get; set; }
        public ICollection<Thesis> Theses { get; set; }

        #endregion

    }
}
