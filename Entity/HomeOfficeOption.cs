
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class HomeOfficeOption : IEntity
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string HomeOfficeName { get; set; }
        [Index]
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
            return CodeName;
        }

        #endregion
    }
}
