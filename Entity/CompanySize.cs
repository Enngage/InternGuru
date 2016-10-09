
namespace Entity
{
    public class CompanySize : IEntity
    {
        public int ID { get; set; }
        public string CompanySizeName { get; set; }
        public string CodeName { get; set; }

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
