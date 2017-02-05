
namespace Entity.Base
{
    public class BaseEntity<TEntity> where TEntity : IEntity
    {
        public IEntity ShallowCopy()
        {
            return (TEntity)this.MemberwiseClone();
        }
    }
}
