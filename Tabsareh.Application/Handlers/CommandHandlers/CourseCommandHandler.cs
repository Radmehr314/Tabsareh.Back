using Tabsareh.Application.Contracts.Commands.Courses;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class CourseCommandHandler :
        ICommandHandler<AddCourseCommand>,
        ICommandHandler<UpdateCourseCommand>,
        ICommandHandler<DeleteCourseCommand>,
        ICommandHandler<AddCourseChapterCommand>,
        ICommandHandler<UpdateCourseChapterCommand>,
        ICommandHandler<DeleteCourseChapterCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(AddCourseCommand command)
        {
            ValidateCourse(command.Title, command.DurationMinutes, command.Price, command.ContentOwnerSharePercent, command.DiscountPercent, command.DiscountStartDate, command.DiscountEndDate);
            await ValidateRelations(command.TeacherId, command.ContentOwnerId, command.CategoryId);

            var course = new Course(
                command.Title,
                command.BannerImage,
                command.DurationMinutes,
                command.CategoryId,
                command.TeacherId,
                command.ContentOwnerId,
                command.Description,
                command.Price,
                command.ContentOwnerSharePercent,
                command.IsActive,
                command.DiscountPercent,
                command.DiscountStartDate,
                command.DiscountEndDate);

            course.SetId(Guid.NewGuid().ToString("N"));
            AddAssets(course, command.SampleVideos, command.PdfFiles);

            var id = await _unitOfWork.CourseRepository.AddAsync(course);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UpdateCourseCommand command)
        {
            ValidateCourse(command.Title, command.DurationMinutes, command.Price, command.ContentOwnerSharePercent, command.DiscountPercent, command.DiscountStartDate, command.DiscountEndDate);

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(command.Id);
            if (course is null || course.IsDeleted) throw new NotFoundException("دوره یافت نشد.");

            await ValidateRelations(command.TeacherId, command.ContentOwnerId, command.CategoryId);

            course.Update(
                command.Title,
                command.BannerImage,
                command.DurationMinutes,
                command.CategoryId,
                command.TeacherId,
                command.ContentOwnerId,
                command.Description,
                command.Price,
                command.ContentOwnerSharePercent,
                command.IsActive,
                command.DiscountPercent,
                command.DiscountStartDate,
                command.DiscountEndDate);

            await _unitOfWork.CourseRepository.UpdateAsync(course);
            await _unitOfWork.CourseRepository.ReplaceAssetsAsync(
                course.Id,
                command.SampleVideos.Select(x => new CourseSampleVideo(course.Id, x.Title, x.FileUrl, x.Duration)).ToList(),
                command.PdfFiles.Select(x => new CoursePdfFile(course.Id, x.Title, x.FileUrl)).ToList());

            return new CommandResult { Id = course.Id };
        }

        public async Task<CommandResult> Handle(DeleteCourseCommand command)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(command.Id);
            if (course is null || course.IsDeleted) throw new NotFoundException("دوره یافت نشد.");

            course.Delete();
            var result = await _unitOfWork.CourseRepository.UpdateAsync(course);
            return new CommandResult { Id = result.Id };
        }

        public async Task<CommandResult> Handle(AddCourseChapterCommand command)
        {
            ValidateChapter(command.Title, command.Duration);
            await ValidateCourseExists(command.CourseId);

            var chapter = new CourseChapter(command.CourseId, command.Title, command.Duration, command.DisplayOrder);
            chapter.SetId(Guid.NewGuid().ToString("N"));
            AddChapterVideos(chapter, command.Videos);
            var id = await _unitOfWork.CourseChapterRepository.AddAsync(chapter);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UpdateCourseChapterCommand command)
        {
            ValidateChapter(command.Title, command.Duration);
            await ValidateCourseExists(command.CourseId);

            var chapter = await _unitOfWork.CourseChapterRepository.GetByIdAsync(command.Id);
            if (chapter is null || chapter.IsDeleted) throw new NotFoundException("سرفصل یافت نشد.");

            chapter.Update(command.CourseId, command.Title, command.Duration, command.DisplayOrder);
            var result = await _unitOfWork.CourseChapterRepository.UpdateAsync(chapter);
            await _unitOfWork.CourseChapterRepository.ReplaceVideosAsync(chapter.Id, ToChapterVideos(chapter.Id, command.Videos));
            return new CommandResult { Id = result.Id };
        }

        public async Task<CommandResult> Handle(DeleteCourseChapterCommand command)
        {
            var chapter = await _unitOfWork.CourseChapterRepository.GetByIdAsync(command.Id);
            if (chapter is null || chapter.IsDeleted) throw new NotFoundException("سرفصل یافت نشد.");

            chapter.Delete();
            var result = await _unitOfWork.CourseChapterRepository.UpdateAsync(chapter);
            return new CommandResult { Id = result.Id };
        }

        private static void AddAssets(Course course, IEnumerable<CourseVideoCommandItem> videos, IEnumerable<CoursePdfCommandItem> pdfs)
        {
            foreach (var video in videos.Where(x => !string.IsNullOrWhiteSpace(x.Title) && !string.IsNullOrWhiteSpace(x.FileUrl)))
                course.SampleVideos.Add(new CourseSampleVideo(course.Id, video.Title, video.FileUrl, video.Duration));

            foreach (var pdf in pdfs.Where(x => !string.IsNullOrWhiteSpace(x.Title) && !string.IsNullOrWhiteSpace(x.FileUrl)))
                course.PdfFiles.Add(new CoursePdfFile(course.Id, pdf.Title, pdf.FileUrl));
        }

        private static void AddChapterVideos(CourseChapter chapter, IEnumerable<CourseChapterVideoCommandItem> videos)
        {
            foreach (var video in ToChapterVideos(chapter.Id, videos))
                chapter.Videos.Add(video);
        }

        private static List<CourseChapterVideo> ToChapterVideos(string chapterId, IEnumerable<CourseChapterVideoCommandItem> videos)
        {
            return videos
                .Where(x => !string.IsNullOrWhiteSpace(x.Title))
                .Select(x => new CourseChapterVideo(
                    chapterId,
                    x.Title,
                    string.IsNullOrWhiteSpace(x.Duration) ? "00:00" : x.Duration,
                    x.ExternalVideoId,
                    x.VideoUrl,
                    string.IsNullOrWhiteSpace(x.UploadStatus) ? "PendingExternalUpload" : x.UploadStatus))
                .ToList();
        }

        private static void ValidateCourse(string title, int durationMinutes, decimal price, decimal sharePercent, decimal? discountPercent, DateTime? discountStartDate, DateTime? discountEndDate)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new UserAccessException("عنوان دوره الزامی است.");
            if (durationMinutes < 0) throw new UserAccessException("مدت زمان دوره معتبر نیست.");
            if (price < 0) throw new UserAccessException("مبلغ دوره معتبر نیست.");
            if (sharePercent < 0 || sharePercent > 100) throw new UserAccessException("سهم صاحب اثر باید بین ۰ تا ۱۰۰ درصد باشد.");
            if (discountPercent.HasValue && (discountPercent < 0 || discountPercent > 100)) throw new UserAccessException("درصد تخفیف باید بین ۰ تا ۱۰۰ باشد.");
            if (discountPercent.HasValue && discountPercent > 0 && (!discountStartDate.HasValue || !discountEndDate.HasValue)) throw new UserAccessException("بازه زمانی تخفیف الزامی است.");
            if (discountStartDate.HasValue && discountEndDate.HasValue && discountStartDate.Value.Date > discountEndDate.Value.Date) throw new UserAccessException("تاریخ شروع تخفیف نمی‌تواند بعد از تاریخ پایان باشد.");
        }

        private static void ValidateChapter(string title, string duration)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new UserAccessException("عنوان سرفصل الزامی است.");
            if (string.IsNullOrWhiteSpace(duration)) throw new UserAccessException("زمان سرفصل الزامی است.");
        }

        private async Task ValidateRelations(string teacherId, string contentOwnerId, string? categoryId)
        {
            var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
            if (teacher is null || teacher.IsDeleted) throw new NotFoundException("استاد یافت نشد.");

            var contentOwner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(contentOwnerId);
            if (contentOwner is null || contentOwner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");

            if (string.IsNullOrWhiteSpace(categoryId)) return;
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category is null || category.IsDeleted) throw new NotFoundException("دسته‌بندی یافت نشد.");
        }

        private async Task ValidateCourseExists(string courseId)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId);
            if (course is null || course.IsDeleted) throw new NotFoundException("دوره یافت نشد.");
        }
    }
}
