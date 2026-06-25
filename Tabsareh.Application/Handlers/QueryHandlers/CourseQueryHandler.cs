using Tabsareh.Application.Contracts.Queries.Course;
using Tabsareh.Application.Contracts.QueryResult.Course;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Domain.Models.Teachers;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class CourseQueryHandler :
        IQueryHandler<GetCoursesPagedQuery, GetCoursesPagedQueryResult>,
        IQueryHandler<GetAllCoursesQuery, List<CourseItemResult>>,
        IQueryHandler<GetCourseByIdQuery, CourseItemResult>,
        IQueryHandler<GetCourseChaptersByCourseIdQuery, List<CourseChapterItemResult>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public async Task<List<CourseChapterItemResult>> Handle(GetCourseChaptersByCourseIdQuery query)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(query.CourseId);
            if (course is null || course.IsDeleted) throw new NotFoundException("دوره یافت نشد.");

            var chapters = await _unitOfWork.CourseChapterRepository.GetByCourseIdAsync(query.CourseId);
            return chapters.Select(x => x.ToItem(course)).ToList();
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
