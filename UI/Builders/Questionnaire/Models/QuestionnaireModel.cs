namespace UI.Builders.Questionnaire.Models
{
    public class QuestionnaireModel
    {
        public int ID { get; set; }
        public string CodeName { get; set; }
        public string ApplicationUserId { get; set; }
        public int CompanyID { get; set; }
        public string QuestionnaireName { get; set; }
        public string QuestionnaireXml { get; set; }
    }
}
