namespace Entity.Base
{
    public interface IEntity
    {
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
