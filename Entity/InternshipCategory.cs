
namespace Entity
{
    public class InternshipCategory : EntityAbstract
    {
        public int ID { get; set; }
        public string CodeName { get; set; }
        public string Name { get; set; }

        #region Entity abstract members

        public override object GetObjectID()
        {
            return ID;
        }

        public override string GetCodeName()
        {
            return CodeName;
        }

        #endregion

    }
}
