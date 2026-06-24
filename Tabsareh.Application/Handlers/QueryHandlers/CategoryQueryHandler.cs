using Tabsareh.Application.Contracts.Queries.Category;
using Tabsareh.Application.Contracts.QueryResult.Category;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class CategoryQueryHandler :
        IQueryHandler<GetCategoriesPagedQuery, GetCategoriesPagedQueryResult>,
        IQueryHandler<GetAllCategoriesQuery, List<CategoryItemResult>>,
        IQueryHandler<GetCategoryByIdQuery, CategoryItemResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetCategoriesPagedQueryResult> Handle(GetCategoriesPagedQuery query)
        {
            var paged = await _unitOfWork.CategoryRepository.GetPagedAsync(query.Options);
            var all = (await _unitOfWork.CategoryRepository.GetAllAsync()).ToDictionary(c => c.Id);

            return new GetCategoriesPagedQueryResult
            {
                Items = paged.Items.Select(c => c.ToItem(all)).ToList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<List<CategoryItemResult>> Handle(GetAllCategoriesQuery query)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            return categories.ToItemList();
        }

        public async Task<CategoryItemResult> Handle(GetCategoryByIdQuery query)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(query.Id);
            if (category is null || category.IsDeleted) throw new NotFoundException("دسته بندی یافت نشد.");

            var all = (await _unitOfWork.CategoryRepository.GetAllAsync()).ToDictionary(c => c.Id);
            return category.ToItem(all);
        }
    }
}
