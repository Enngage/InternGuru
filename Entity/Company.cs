
namespace Entity
{
    public class Company : EntityAbstract
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public int Founded { get; set; }
        public string About { get; set; }
        public int Address { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string Web { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string Facebook { get; set; }


        #region Virtual properties


        #endregion
    }
}
