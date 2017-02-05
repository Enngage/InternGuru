using System;

namespace Entity.Base
{
    public interface IEntityWithGuid
    {
        /// <summary>
        /// Guid of the entity
        /// </summary>
        Guid Guid { get; set; }
    }
}
