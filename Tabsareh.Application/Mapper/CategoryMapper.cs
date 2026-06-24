using Tabsareh.Application.Contracts.QueryResult.Category;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Framework.Application.Convertor;

namespace Tabsareh.Application.Mapper;

public static class CategoryMapper
{
    public static CategoryItemResult ToItem(this Category category, IReadOnlyDictionary<string, Category>? categories = null)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        Category? parent = null;
        if (!string.IsNullOrWhiteSpace(category.ParentId))
            categories?.TryGetValue(category.ParentId, out parent);

        return new CategoryItemResult
        {
            Id = category.Id,
            Name = category.Name,
            ParentId = category.ParentId,
            ParentName = parent?.Name,
            Level = GetLevel(category, categories),
            CreatedAt = category.CreatedAt.ToShamsiDate()
        };
    }

    public static List<CategoryItemResult> ToItemList(this IEnumerable<Category> categories)
    {
        var list = categories.ToList();
        var map = list.ToDictionary(c => c.Id);
        var children = list
            .GroupBy(c => c.ParentId ?? string.Empty)
            .ToDictionary(g => g.Key, g => g.OrderBy(c => c.Name).ToList());

        var result = new List<CategoryItemResult>();
        AddTreeItems(string.Empty, children, map, result);
        return result;
    }

    private static void AddTreeItems(
        string parentId,
        IReadOnlyDictionary<string, List<Category>> children,
        IReadOnlyDictionary<string, Category> map,
        List<CategoryItemResult> result)
    {
        if (!children.TryGetValue(parentId, out var items)) return;

        foreach (var item in items)
        {
            result.Add(item.ToItem(map));
            AddTreeItems(item.Id, children, map, result);
        }
    }

    private static int GetLevel(Category category, IReadOnlyDictionary<string, Category>? categories)
    {
        if (categories == null) return 0;

        var level = 0;
        var current = category;
        var visited = new HashSet<string> { category.Id };

        while (!string.IsNullOrWhiteSpace(current.ParentId) &&
               categories.TryGetValue(current.ParentId, out var parent) &&
               visited.Add(parent.Id))
        {
            level++;
            current = parent;
        }

        return level;
    }
}
