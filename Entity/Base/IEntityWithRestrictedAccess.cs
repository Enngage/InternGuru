
namespace Entity.Base
{
    /// <summary>
    /// Represents entities which can be updated or deleted only by its creator
    /// </summary>
    public interface IEntityWithRestrictedAccess : IEntityWithUserStamp
    {
    }
}
