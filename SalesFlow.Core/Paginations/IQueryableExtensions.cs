
using Microsoft.EntityFrameworkCore;

namespace SalesFlow.Core.Paginations
{

    public static class IQueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, PaginationRequest request)
        {
            int totalCount = await query.CountAsync();
            var items = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}