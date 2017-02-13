
using System;

namespace Entity.Base
{
    /// <summary>
    /// Represents entities with active state
    /// </summary>
    public interface IEntityWithActiveState
    {
        DateTime? ActiveSince { get; set; }
        bool IsActive { get; set; }
    }
}
