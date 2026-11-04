

namespace Smartup.M2M.BuildingBlocks.Contracts.Pagination
{
    public class PaginatedResponse<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public long TotalCount { get; }
        public int TotalPages { get; }
        public bool HasPrevious => PageIndex > 1;
        public bool HasNext => PageIndex < TotalPages;

        public PaginatedResponse(IReadOnlyList<T> items, long totalCount, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
