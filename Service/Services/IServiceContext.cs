using System;

namespace Service.Services
{
    public interface IServiceContext : IDisposable
    {

        /// <summary>
        /// Indicates if permissions will be checked 
        /// </summary>
        bool CheckPermissions { get; set; }
    }
}
