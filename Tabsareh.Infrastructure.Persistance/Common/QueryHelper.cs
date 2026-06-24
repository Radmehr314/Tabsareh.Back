using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;

namespace Tabsareh.Infrastructure.Persistance.Common;

public static class QueryHelper
{
    /// <summary>
    /// صفحه‌بندی و مرتب‌سازی روی یک IQueryable و برگرداندن PagedResult.
    /// </summary>
    public static async Task<PagedResult<TEntity>> GetPagedResultAsync<TEntity>(
        IQueryable<TEntity> query,
        QueryOptions options) where TEntity : class
    {
        var totalCount = await query.LongCountAsync();

        var sortBy = string.IsNullOrWhiteSpace(options.SortBy) ? "CreatedAt" : options.SortBy;
        query = ApplyOrder(query, sortBy, options.Descending);

        var items = await query.Skip(options.Skip).Take(options.Limit).ToListAsync();

        return new PagedResult<TEntity>
        {
            Items = items,
            TotalCount = totalCount,
            Skip = options.Skip,
            Limit = options.Limit
        };
    }

    private static IQueryable<TEntity> ApplyOrder<TEntity>(IQueryable<TEntity> query, string propertyName, bool descending)
    {
        var prop = typeof(TEntity).GetProperty(propertyName,
            System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        if (prop is null)
            return query; // اگر ستون نامعتبر بود بدون مرتب‌سازی برگردان

        var param = Expression.Parameter(typeof(TEntity), "x");
        var body = Expression.Convert(Expression.Property(param, prop), typeof(object));
        var selector = Expression.Lambda<Func<TEntity, object>>(body, param);

        return descending ? query.OrderByDescending(selector) : query.OrderBy(selector);
    }
}
