

namespace Smartup.M2M.BuildingBlocks.Contracts.Pagination
{
    public record PageRequest(string? Criteria = null, int PageNumber = 1, int PageSize = 10);
}
