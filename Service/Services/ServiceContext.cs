
namespace Service.Services
{
    public sealed class ServiceContext : IServiceContext
    {
        public ServiceContext()
        {
            SetDefaultValues();   
        }
        public bool CheckPermissions { get; set; }
        public void Dispose()
        {
            // restore default values
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            CheckPermissions = true;
        }
    }
}
