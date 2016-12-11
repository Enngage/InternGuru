
namespace UI.Builders.Shared.Models
{
    /// <summary>
    /// Integer wrapper class used because cache service can cache only reference types
    /// </summary>
    public class IntWrapper
    {
        public int Value { get; set; }
    }
}
