
namespace Service.Services
{
    public interface IInsertActionResult
    {
        int ObjectID { get; }
        int UpdatedRows { get; }
    }
}
