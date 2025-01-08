using ClickSlotDAL.Contracts.Interfaces;

namespace ClickSlotDAL.Entities.Base
{
    public abstract class EntityBase<T> : IEntity
    {
        public T Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not EntityBase<T> other)
            {
                return false;
            }
            return EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Id);
        }
    }
}