using MongoDB.Driver;
using Tabsareh.Domain.Common;

namespace Tabsareh.Infrastructure.Persistance.Common;

public class QueryHelper
{
    public static async Task<List<TDocument>> ApplyPagingAndSortingAsync<TDocument>(
        IMongoCollection<TDocument> collection,
        FilterDefinition<TDocument> filter,
        int skip,
        int limit,
        string? sortBy,
        bool descending)
    {
        var query = collection.Find(filter);
        if (!string.IsNullOrEmpty(sortBy))
        {
            var sortDef = descending
                ? Builders<TDocument>.Sort.Descending(sortBy)
                : Builders<TDocument>.Sort.Ascending(sortBy);
            query = query.Sort(sortDef);
        }

        return await query.Skip(skip).Limit(limit).ToListAsync();
    }

    public static async Task<PagedResult<TDocument>> GetPagedResultAsync<TDocument>(
        IMongoCollection<TDocument> collection,
        FilterDefinition<TDocument> filter,
        QueryOptions options)
    {
        var totalCount = await collection.CountDocumentsAsync(filter);
        var items = await ApplyPagingAndSortingAsync(collection, filter, options.Skip, options.Limit, options.SortBy,
            options.Descending);
        return new PagedResult<TDocument>
        {
            Items = items,
            TotalCount = totalCount,
            Skip = options.Skip,
            Limit = options.Limit
        };
    }
}
