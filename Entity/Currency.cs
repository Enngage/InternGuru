
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;
using Entity.Base;

namespace Entity
{
    public class Currency : IEntity, IEntityWithUniqueCodeName
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string CurrencyName { get; set; }
        [Index]
        [MaxLength(50)]
        public string CodeName { get; set; }
        public bool ShowSignOnLeft { get; set; }

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }

        public string GetCodeName()
        {
            return StringHelper.GetCodeName(CurrencyName);
        }

        #endregion
    }
}
