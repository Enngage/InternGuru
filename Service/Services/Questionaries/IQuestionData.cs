
namespace Service.Services.Questionaries
{
    public interface IQuestionData
    {
        /// <summary>
        /// Name of data placeholder
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Value stored in the placeholder
        /// </summary>
        string Value { get; set; }

    }
}
