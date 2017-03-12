namespace Service.Services.Questionnaires
{
    public interface IQuestionSubmit
    {
        string Guid { get; set; }
        string QuestionType { get; set; }
        string QuestionText { get; set; }
        string Answer { get; set; }
        string CorrectAnswer { get; set; }
        QuestionAnswerResultEnum Result { get; }
        bool IsTestQuestion { get; }
    }
}
