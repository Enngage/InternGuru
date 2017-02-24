
namespace Entity.Base
{
    /// <summary>
    /// Represents entities with optional information about who created/updated user
    /// </summary>
    public interface IEntityWithOptionalUserStamp
    {
        string CreatedByApplicationUserId { get; set; }
        string UpdatedByApplicationUserId { get; set; }

        ApplicationUser CreatedByApplicationUser { get; set; }
        ApplicationUser UpdatedByApplicationUser { get; set; }
    }
}
