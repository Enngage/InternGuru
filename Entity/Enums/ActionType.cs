
namespace Entity.Enums
{
    internal sealed class ActionType
    {
        public static readonly ActionType Update = new ActionType("Update");
        public static readonly ActionType UpdateAny = new ActionType("UpdateAny");
        public static readonly ActionType DeleteAny = new ActionType("DeleteAny");
        public static readonly ActionType Delete = new ActionType("Delete");
        public static readonly ActionType CreateAny = new ActionType("CreateAny");

        private ActionType(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}
