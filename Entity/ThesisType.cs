
using Common.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class ThesisType : IEntity
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [Index]
        [MaxLength(50)]
        public string CodeName { get; set; }

        #region Virtual properties

        [ForeignKey("ThesisID")] // Thesis model needs to point here
        public ICollection<Thesis> Thesis { get; set; }

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
