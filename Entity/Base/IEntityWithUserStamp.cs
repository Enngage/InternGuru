
namespace Entity.Base
{
    /// <summary>
    /// Represents entities with information about who created/updated user
    /// </summary>
    public interface IEntityWithUserStamp
    {
        string CreatedByApplicationUserId { get; set; }
        string UpdatedByApplicationUserId { get; set; }

        ApplicationUser CreatedByApplicationUser { get; set; }
        ApplicationUser UpdatedByApplicationUser { get; set; }
    }
}
