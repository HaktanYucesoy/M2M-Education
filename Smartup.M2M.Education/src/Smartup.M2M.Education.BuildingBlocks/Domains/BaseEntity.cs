

namespace Smartup.M2M.Education.BuildingBlocks.Domains
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    }
}
