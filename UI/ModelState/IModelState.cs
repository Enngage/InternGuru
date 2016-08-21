using System.Collections.Generic;

namespace UI.ModelState
{
    public interface IModelState
    {
        void AddError(string key, string errorMessage);

        void AddError(string errorMessage);

        void AddErrors(IEnumerable<string> errorMessages);
        bool IsValid { get; }
    }
}