using Tabsareh.Application.Contracts.QueryResult.DynamicPage;
using Tabsareh.Domain.Models.DynamicPages;
using Tabsareh.Framework.Application.Convertor;

namespace Tabsareh.Application.Mapper;

public static class DynamicPageMapper
{
    public static DynamicPageItemResult ToItem(this DynamicPage page)
    {
        if (page == null)
            throw new ArgumentNullException(nameof(page));

        return new DynamicPageItemResult
        {
            Id = page.Id,
            Title = page.Title,
            Slug = page.Slug,
            Body = page.Body,
            MetaTitle = page.MetaTitle,
            MetaDescription = page.MetaDescription,
            MetaKeywords = page.MetaKeywords,
            DisplayOrder = page.DisplayOrder,
            IsPublished = page.IsPublished,
            PublishedAt = page.PublishedAt?.ToShamsiDate(),
            CreatedAt = page.CreatedAt.ToShamsiDate()
        };
    }

    public static DynamicPageMenuItemResult ToMenuItem(this DynamicPage page)
    {
        if (page == null)
            throw new ArgumentNullException(nameof(page));

        return new DynamicPageMenuItemResult
        {
            Title = page.Title,
            Slug = page.Slug
        };
    }
}
