namespace UI.Builders.Questionare.Models
{
    public class QuestionareModel
    {
        public int ID { get; set; }
        public string CodeName { get; set; }
        public string ApplicationUserId { get; set; }
        public int CompanyID { get; set; }
        public string QuestionareName { get; set; }
        public string QuestionareDefinitionXml { get; set; }
    }
}
