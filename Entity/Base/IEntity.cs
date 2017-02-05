namespace Entity.Base
{
    public interface IEntity
    {

        /// <summary>
        /// ID of the entity
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Code name of the entity
        /// </summary>
        string CodeName { get; set; }

        /// <summary>
        /// Gets value from entity primary key
        /// </summary>
        object GetObjectID();

        /// <summary>
        /// Gets code name value
        /// </summary>
        string GetCodeName();
    }
}
