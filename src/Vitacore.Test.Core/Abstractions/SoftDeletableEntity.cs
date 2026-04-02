using Vitacore.Test.Core.Exceptions;

namespace Vitacore.Test.Core.Abstractions
{
    public abstract class SoftDeletableEntity : BaseEntity, ISoftDeletable
    {
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public void MarkAsDeleted(DateTime deletedAt)
        {
            if (deletedAt == default)
            {
                throw new RequiredFieldNotSpecifiedException(nameof(DeletedAt));
            }

            IsDeleted = true;
            DeletedAt = deletedAt;
        }
    }
}
