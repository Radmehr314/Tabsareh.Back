using Tabsareh.Application.Contracts.Queries.Blog;
using Tabsareh.Application.Contracts.QueryResult.Blog;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class BlogQueryHandler :
        IQueryHandler<GetBlogsPagedQuery, GetBlogsPagedQueryResult>,
        IQueryHandler<GetAllBlogsQuery, List<BlogItemResult>>,
        IQueryHandler<GetBlogByIdQuery, BlogItemResult>,
        IQueryHandler<GetBlogBySlugQuery, BlogItemResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetBlogsPagedQueryResult> Handle(GetBlogsPagedQuery query)
        {
            var paged = await _unitOfWork.BlogRepository.GetPagedAsync(query.Options);
            var categories = await GetCategoryMap();

            return new GetBlogsPagedQueryResult
            {
                Items = paged.Items.Select(blog => blog.ToItem(GetCategory(blog.CategoryId, categories))).ToList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<List<BlogItemResult>> Handle(GetAllBlogsQuery query)
        {
            var blogs = await _unitOfWork.BlogRepository.GetAllAsync();
            var categories = await GetCategoryMap();
            return blogs.Select(blog => blog.ToItem(GetCategory(blog.CategoryId, categories))).ToList();
        }

        public async Task<BlogItemResult> Handle(GetBlogByIdQuery query)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(query.Id);
            if (blog is null || blog.IsDeleted) throw new NotFoundException("بلاگ یافت نشد.");

            Category? category = null;
            if (!string.IsNullOrWhiteSpace(blog.CategoryId))
                category = await _unitOfWork.CategoryRepository.GetByIdAsync(blog.CategoryId);

            return blog.ToItem(category);
        }

        public async Task<BlogItemResult> Handle(GetBlogBySlugQuery query)
        {
            var blog = await _unitOfWork.BlogRepository.GetBySlugAsync(query.Slug);
            if (blog is null || blog.IsDeleted) throw new NotFoundException("بلاگ یافت نشد.");

            Category? category = null;
            if (!string.IsNullOrWhiteSpace(blog.CategoryId))
                category = await _unitOfWork.CategoryRepository.GetByIdAsync(blog.CategoryId);

            return blog.ToItem(category);
        }

        private async Task<Dictionary<string, Category>> GetCategoryMap()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            return categories.ToDictionary(c => c.Id);
        }

        private static Category? GetCategory(string? categoryId, IReadOnlyDictionary<string, Category> categories)
        {
            if (string.IsNullOrWhiteSpace(categoryId)) return null;
            categories.TryGetValue(categoryId, out var category);
            return category;
        }
    }
}
