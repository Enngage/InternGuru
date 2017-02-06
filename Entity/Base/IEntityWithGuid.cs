using System;

namespace Entity.Base
{
    /// <summary>
    /// Represents entities with Guid
    /// </summary>
    public interface IEntityWithGuid
    {
        /// <summary>
        /// Guid of the entity
        /// </summary>
        Guid Guid { get; set; }
    }
}
