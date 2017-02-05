using System;

namespace Entity.Base
{
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
