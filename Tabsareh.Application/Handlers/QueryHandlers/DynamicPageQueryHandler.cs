using Tabsareh.Application.Contracts.Queries.DynamicPage;
using Tabsareh.Application.Contracts.QueryResult.DynamicPage;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class DynamicPageQueryHandler :
        IQueryHandler<GetDynamicPagesPagedQuery, GetDynamicPagesPagedQueryResult>,
        IQueryHandler<GetAllDynamicPagesQuery, List<DynamicPageItemResult>>,
        IQueryHandler<GetDynamicPageByIdQuery, DynamicPageItemResult>,
        IQueryHandler<GetDynamicPageBySlugQuery, DynamicPageItemResult>,
        IQueryHandler<GetPublishedDynamicPageMenuQuery, List<DynamicPageMenuItemResult>>,
        IQueryHandler<GetPublishedDynamicPageBySlugQuery, DynamicPageItemResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DynamicPageQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetDynamicPagesPagedQueryResult> Handle(GetDynamicPagesPagedQuery query)
        {
            var paged = await _unitOfWork.DynamicPageRepository.GetPagedAsync(query.Options);

            return new GetDynamicPagesPagedQueryResult
            {
                Items = paged.Items.Select(page => page.ToItem()).ToList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<List<DynamicPageItemResult>> Handle(GetAllDynamicPagesQuery query)
        {
            var pages = await _unitOfWork.DynamicPageRepository.GetAllAsync();
            return pages.Select(page => page.ToItem()).ToList();
        }

        public async Task<DynamicPageItemResult> Handle(GetDynamicPageByIdQuery query)
        {
            var page = await _unitOfWork.DynamicPageRepository.GetByIdAsync(query.Id);
            if (page is null || page.IsDeleted) throw new NotFoundException("صفحه یافت نشد.");
            return page.ToItem();
        }

        public async Task<DynamicPageItemResult> Handle(GetDynamicPageBySlugQuery query)
        {
            var page = await _unitOfWork.DynamicPageRepository.GetBySlugAsync(query.Slug);
            if (page is null || page.IsDeleted) throw new NotFoundException("صفحه یافت نشد.");
            return page.ToItem();
        }

        public async Task<List<DynamicPageMenuItemResult>> Handle(GetPublishedDynamicPageMenuQuery query)
        {
            var pages = await _unitOfWork.DynamicPageRepository.GetAllAsync(onlyPublished: true);
            return pages.Select(page => page.ToMenuItem()).ToList();
        }

        public async Task<DynamicPageItemResult> Handle(GetPublishedDynamicPageBySlugQuery query)
        {
            var page = await _unitOfWork.DynamicPageRepository.GetBySlugAsync(query.Slug, onlyPublished: true);
            if (page is null || page.IsDeleted) throw new NotFoundException("صفحه یافت نشد.");
            return page.ToItem();
        }
    }
}
