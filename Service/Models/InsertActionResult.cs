
using Service.Services;

namespace Service.Models
{
    public class InsertActionResult : IInsertActionResult
    {
        public int ObjectID { get; set; }
        public int UpdatedRows { get; set; }
    }
}
