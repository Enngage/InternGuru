using System;

namespace Entity.Base
{
    /// <summary>
    /// Represents entities that contain time stamp of their creation and last update
    /// </summary>
    public interface IEntityWithTimeStamp
    {
        /// <summary>
        /// Time when entity object was created
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// Time when entity object was updated
        /// </summary>
        DateTime Updated { get; set; }
    }
}
