namespace Tabsareh.Domain.Models.Courses
{
    public class CourseChapterVideo : BaseEntity<string>
    {
        private CourseChapterVideo() { }

        public CourseChapterVideo(
            string courseChapterId,
            string title,
            string duration,
            string? externalVideoId,
            string? videoUrl,
            string uploadStatus)
        {
            CourseChapterId = courseChapterId;
            Title = title;
            Duration = duration;
            ExternalVideoId = externalVideoId;
            VideoUrl = videoUrl;
            UploadStatus = uploadStatus;
        }

        public void SetId(string id) => Id = id;

        public string CourseChapterId { get; private set; }
        public string Title { get; private set; }
        public string Duration { get; private set; }
        public string? ExternalVideoId { get; private set; }
        public string? VideoUrl { get; private set; }
        public string UploadStatus { get; private set; }
    }
}
