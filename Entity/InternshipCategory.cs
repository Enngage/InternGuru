using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity
{
    public class InternshipCategory : IEntity
    {

        #region DB 

        public int ID { get; set; }
        [Index]
        [MaxLength(50)]
        public string CodeName { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }

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

        #region Virtual properties

        public ICollection<Internship> Internships { get; set; }
        public ICollection<Thesis> Theses { get; set; }

        #endregion

    }
}
