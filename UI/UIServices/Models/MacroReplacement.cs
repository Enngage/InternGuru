
namespace UI.UIServices.Models
{
    /// <summary>
    /// Model representing replacement of macros
    /// </summary>
    public class MacroReplacement
    {

        /// <summary>
        /// Macro to replace
        /// </summary>
        public string MacroName { get; set; }

        /// <summary>
        /// Replacement value
        /// </summary>
        public string Value { get; set; }

        public override string ToString()
        {
            return $"Macro: {MacroName}: {Value}";
        }
    }
}
