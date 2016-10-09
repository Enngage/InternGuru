
namespace Core.Context
{
    public class AppContextConfig
    {
        private bool autoDetectChanges = false; // default value

        public bool AutoDetectChanges
        {
            get
            {
                return autoDetectChanges;
            }
            set
            {
                this.autoDetectChanges = value;
            }
        }
    }
}
