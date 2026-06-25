namespace Tabsareh.Domain.Models.Courses
{
    public class CourseSampleVideo : BaseEntity<string>
    {
        private CourseSampleVideo() { }

        public CourseSampleVideo(string courseId, string title, string fileUrl, string duration)
        {
            CourseId = courseId;
            Title = title;
            FileUrl = fileUrl;
            Duration = duration;
        }

        public void SetId(string id) => Id = id;

        public string CourseId { get; private set; }
        public string Title { get; private set; }
        public string FileUrl { get; private set; }
        public string Duration { get; private set; }
    }
}
