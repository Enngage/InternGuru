namespace Entity.Base
{
    public interface IEntity
    {

        /// <summary>
        /// ID of the entity
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Gets code name value of object
        /// </summary>
        string GetCodeName();

        /// <summary>
        /// Gets value from entity primary key
        /// </summary>
        object GetObjectID();
    }
}
