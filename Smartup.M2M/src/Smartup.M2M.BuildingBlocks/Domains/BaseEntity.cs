

namespace Smartup.M2M.BuildingBlocks.Domains
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.CreateVersion7();

        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    }
}
