using Tabsareh.Application.Contracts.QueryResult.Course;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Domain.Models.Teachers;
using Tabsareh.Framework.Application.Convertor;

namespace Tabsareh.Application.Mapper
{
    public static class CourseMapper
    {
        public static CourseItemResult ToItem(
            this Course course,
            Category? category,
            Teacher? teacher,
            ContentOwner? contentOwner)
        {
            return new CourseItemResult
            {
                Id = course.Id,
                Title = course.Title,
                BannerImage = course.BannerImage,
                DurationMinutes = course.DurationMinutes,
                CategoryId = course.CategoryId,
                CategoryName = category?.Name,
                TeacherId = course.TeacherId,
                TeacherName = teacher == null ? null : $"{teacher.FirstName} {teacher.LastName}".Trim(),
                ContentOwnerId = course.ContentOwnerId,
                ContentOwnerName = contentOwner?.Name,
                Description = course.Description,
                Price = course.Price,
                SettlementBasePrice = course.SettlementBasePrice,
                ContentOwnerSharePercent = course.ContentOwnerSharePercent,
                DiscountPercent = course.DiscountPercent,
                DiscountStartDate = course.DiscountStartDate,
                DiscountEndDate = course.DiscountEndDate,
                DiscountStartDatePersian = course.DiscountStartDate?.ToShamsiDate(),
                DiscountEndDatePersian = course.DiscountEndDate?.ToShamsiDate(),
                IsActive = course.IsActive,
                CreatedAt = course.CreatedAt,
                SampleVideos = course.SampleVideos.Select(x => new CourseVideoResult
                {
                    Id = x.Id,
                    Title = x.Title,
                    FileUrl = x.FileUrl,
                    Duration = x.Duration
                }).ToList(),
                PdfFiles = course.PdfFiles.Select(x => new CoursePdfResult
                {
                    Id = x.Id,
                    Title = x.Title,
                    FileUrl = x.FileUrl
                }).ToList()
            };
        }

        public static CourseChapterItemResult ToItem(this CourseChapter chapter, Course? course)
        {
            return new CourseChapterItemResult
            {
                Id = chapter.Id,
                CourseId = chapter.CourseId,
                CourseTitle = course?.Title,
                Title = chapter.Title,
                Duration = chapter.Duration,
                DisplayOrder = chapter.DisplayOrder,
                CreatedAt = chapter.CreatedAt.ToShamsiDate(),
                Videos = chapter.Videos.Select(x => new CourseChapterVideoResult
                {
                    Id = x.Id,
                    Title = x.Title,
                    Duration = x.Duration,
                    ExternalVideoId = x.ExternalVideoId,
                    VideoUrl = x.VideoUrl,
                    UploadStatus = x.UploadStatus
                }).ToList()
            };
        }
    }
}
