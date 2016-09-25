
namespace Entity
{
    public class InternshipCategory : IEntity
    {
        public int ID { get; set; }
        public string CodeName { get; set; }
        public string Name { get; set; }

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
