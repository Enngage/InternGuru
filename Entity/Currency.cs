
namespace Entity
{
    public class Currency : IEntity
    {
        public int ID { get; set; }
        public string CurrencyName { get; set; }
        public string CodeName { get; set; }
        public bool ShowSignOnLeft { get; set; }

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
