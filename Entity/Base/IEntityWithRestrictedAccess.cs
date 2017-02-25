
namespace Entity.Base
{
    /// <summary>
    /// Represents entities which can be updated or deleted only by its creator
    /// </summary>
    public interface IEntityWithRestrictedAccess 
    {
        string CreatedByApplicationUserId { get; set; }
        string UpdatedByApplicationUserId { get; set; }

        ApplicationUser CreatedByApplicationUser { get; set; }
        ApplicationUser UpdatedByApplicationUser { get; set; }
    }
}
