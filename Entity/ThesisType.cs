using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class ThesisType : IEntity, IEntityWithUniqueCodeName
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [Index]
        [Required]
        [MaxLength(50)]
        public string CodeName { get; set; }

        #region Virtual properties

        [ForeignKey("ThesisID")] // Thesis model needs to point here
        public ICollection<Thesis> Theses { get; set; }

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
    }
}
