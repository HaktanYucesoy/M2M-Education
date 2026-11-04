using Smartup.M2M.BuildingBlocks.Contracts.Pagination;
using Microsoft.EntityFrameworkCore;


namespace Smartup.M2M.Infrastructure.Persistence.EfCore.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedResponse<T>> ToPaginatedResponseAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var count = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResponse<T>(items, count, pageNumber, pageSize);
        }
    }
}
