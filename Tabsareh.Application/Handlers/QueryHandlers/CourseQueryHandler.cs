using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Contracts.Queries.Course;
using Tabsareh.Application.Contracts.Queries.ContentOwner;
using Tabsareh.Application.Contracts.QueryResult.Course;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Domain.Models.SiteSettings;
using Tabsareh.Domain.Models.Teachers;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class CourseQueryHandler :
        IQueryHandler<GetCoursesPagedQuery, GetCoursesPagedQueryResult>,
        IQueryHandler<GetAllCoursesQuery, List<CourseItemResult>>,
        IQueryHandler<GetCourseByIdQuery, CourseItemResult>,
        IQueryHandler<GetCourseChaptersByCourseIdQuery, List<CourseChapterItemResult>>,
        IQueryHandler<GetMyCoursesQuery, List<CourseItemResult>>,
        IQueryHandler<GetMyCourseChaptersQuery, List<CourseChapterItemResult>>,
        IQueryHandler<GetPublicCoursesQuery, PublicCoursesResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoService _userInfoService;

        public CourseQueryHandler(IUnitOfWork unitOfWork, IUserInfoService userInfoService)
        {
            _unitOfWork = unitOfWork;
            _userInfoService = userInfoService;
        }

        public async Task<GetCoursesPagedQueryResult> Handle(GetCoursesPagedQuery query)
        {
            var paged = await _unitOfWork.CourseRepository.GetPagedAsync(query.Options);
            var maps = await GetMaps();

            return new GetCoursesPagedQueryResult
            {
                Items = paged.Items.Select(x => x.ToItem(Get(x.CategoryId, maps.Categories), Get(x.TeacherId, maps.Teachers), Get(x.ContentOwnerId, maps.ContentOwners))).ToList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<List<CourseItemResult>> Handle(GetAllCoursesQuery query)
        {
            var courses = await _unitOfWork.CourseRepository.GetAllAsync();
            var maps = await GetMaps();
            return courses.Select(x => x.ToItem(Get(x.CategoryId, maps.Categories), Get(x.TeacherId, maps.Teachers), Get(x.ContentOwnerId, maps.ContentOwners))).ToList();
        }

        public async Task<CourseItemResult> Handle(GetCourseByIdQuery query)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(query.Id);
            if (course is null || course.IsDeleted) throw new NotFoundException("دوره یافت نشد.");

            var category = string.IsNullOrWhiteSpace(course.CategoryId) ? null : await _unitOfWork.CategoryRepository.GetByIdAsync(course.CategoryId);
            var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(course.TeacherId);
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(course.ContentOwnerId);
            return course.ToItem(category, teacher, owner);
        }

        public async Task<List<CourseChapterItemResult>> Handle(GetMyCourseChaptersQuery query)
        {
            var ownerId = _userInfoService.GetUserIdByToken();
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(query.CourseId);
            if (course is null || course.IsDeleted || course.ContentOwnerId != ownerId)
                throw new NotFoundException("دوره یافت نشد.");

            var chapters = await _unitOfWork.CourseChapterRepository.GetByCourseIdAsync(query.CourseId);
            return chapters.Select(x => x.ToItem(course)).ToList();
        }

        public async Task<List<CourseItemResult>> Handle(GetMyCoursesQuery query)
        {
            var ownerId = _userInfoService.GetUserIdByToken();
            var courses = await _unitOfWork.CourseRepository.GetAllAsync();
            var myCourses = courses.Where(x => !x.IsDeleted && x.ContentOwnerId == ownerId).ToList();
            var maps = await GetMaps();
            return myCourses.Select(x => x.ToItem(Get(x.CategoryId, maps.Categories), Get(x.TeacherId, maps.Teachers), Get(x.ContentOwnerId, maps.ContentOwners))).ToList();
        }

        public async Task<List<CourseChapterItemResult>> Handle(GetCourseChaptersByCourseIdQuery query)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(query.CourseId);
            if (course is null || course.IsDeleted) throw new NotFoundException("دوره یافت نشد.");

            var chapters = await _unitOfWork.CourseChapterRepository.GetByCourseIdAsync(query.CourseId);
            return chapters.Select(x => x.ToItem(course)).ToList();
        }

        public async Task<PublicCoursesResult> Handle(GetPublicCoursesQuery query)
        {
            var licensePriceRaw = await _unitOfWork.SiteSettingRepository.GetAsync(SiteSettingKeys.LicensePrice);
            var licensePrice = decimal.TryParse(licensePriceRaw, out var lp) ? lp : 0m;

            var courses = await _unitOfWork.CourseRepository.GetAllAsync();
            var activeCourses = courses.Where(x => !x.IsDeleted && x.IsActive).ToList();
            var maps = await GetMaps();
            var today = DateTime.Now.Date;

            var items = activeCourses.Select(x =>
            {
                var effectiveDiscount = GetActiveDiscountPercent(x, today);
                var discountedPrice = Math.Max(0, x.Price - Math.Round(x.Price * effectiveDiscount / 100));
                return new PublicCourseItem
                {
                    Id = x.Id,
                    Title = x.Title,
                    BannerImage = x.BannerImage,
                    DurationMinutes = x.DurationMinutes,
                    CategoryName = Get(x.CategoryId, maps.Categories)?.Name,
                    TeacherName = Get(x.TeacherId, maps.Teachers) is { } t ? $"{t.FirstName} {t.LastName}".Trim() : null,
                    Description = x.Description,
                    Price = x.Price,
                    LicensePrice = licensePrice,
                    TotalPrice = discountedPrice + licensePrice,
                    DiscountPercent = x.DiscountPercent,
                    EffectiveDiscountPercent = effectiveDiscount > 0 ? effectiveDiscount : null,
                    DiscountStartDate = x.DiscountStartDate,
                    DiscountEndDate = x.DiscountEndDate,
                    SampleVideos = x.SampleVideos.Select(v => new CourseVideoResult { Id = v.Id, Title = v.Title, FileUrl = v.FileUrl, Duration = v.Duration }).ToList(),
                    PdfFiles = x.PdfFiles.Select(p => new CoursePdfResult { Id = p.Id, Title = p.Title, FileUrl = p.FileUrl }).ToList(),
                    AverageRating = x.AverageRating,
                    CommentCount = x.CommentCount
                };
            }).ToList();

            return new PublicCoursesResult { LicensePrice = licensePrice, Courses = items };
        }

        private static decimal GetActiveDiscountPercent(Course course, DateTime today)
        {
            if (!course.DiscountPercent.HasValue || course.DiscountPercent <= 0) return 0;
            if (course.DiscountStartDate.HasValue && today < course.DiscountStartDate.Value.Date) return 0;
            if (course.DiscountEndDate.HasValue && today > course.DiscountEndDate.Value.Date) return 0;
            return course.DiscountPercent.Value;
        }

        private async Task<(Dictionary<string, Category> Categories, Dictionary<string, Teacher> Teachers, Dictionary<string, ContentOwner> ContentOwners)> GetMaps()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
            var owners = await _unitOfWork.ContentOwnerRepository.GetAllAsync();
            return (categories.ToDictionary(x => x.Id), teachers.ToDictionary(x => x.Id), owners.ToDictionary(x => x.Id));
        }

        private static T? Get<T>(string? id, IReadOnlyDictionary<string, T> map) where T : class
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            map.TryGetValue(id, out var item);
            return item;
        }
    }
}
