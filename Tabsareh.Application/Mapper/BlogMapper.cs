using Tabsareh.Application.Contracts.QueryResult.Blog;
using Tabsareh.Domain.Models.Blogs;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Framework.Application.Convertor;

namespace Tabsareh.Application.Mapper;

public static class BlogMapper
{
    public static BlogItemResult ToItem(this Blog blog, Category? category = null)
    {
        if (blog == null)
            throw new ArgumentNullException(nameof(blog));

        return new BlogItemResult
        {
            Id = blog.Id,
            Title = blog.Title,
            Slug = blog.Slug,
            TitleImage = blog.TitleImage,
            Body = blog.Body,
            Excerpt = blog.Excerpt,
            CategoryId = blog.CategoryId,
            CategoryName = category?.Name,
            MetaTitle = blog.MetaTitle,
            MetaDescription = blog.MetaDescription,
            MetaKeywords = blog.MetaKeywords,
            IsPublished = blog.IsPublished,
            PublishedAt = blog.PublishedAt?.ToShamsiDate(),
            CreatedAt = blog.CreatedAt.ToShamsiDate()
        };
    }
}
