
namespace Core.Loc
{
    public class ConstructorParameter
    { 
        public string ParamName { get; }
        public object Value { get; }

        public ConstructorParameter(string paramName, object value)
        {
            ParamName = paramName;
            Value = value;
        }
    }
}
