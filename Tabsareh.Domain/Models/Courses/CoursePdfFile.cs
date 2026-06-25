namespace Tabsareh.Domain.Models.Courses
{
    public class CoursePdfFile : BaseEntity<string>
    {
        private CoursePdfFile() { }

        public CoursePdfFile(string courseId, string title, string fileUrl)
        {
            CourseId = courseId;
            Title = title;
            FileUrl = fileUrl;
        }

        public void SetId(string id) => Id = id;

        public string CourseId { get; private set; }
        public string Title { get; private set; }
        public string FileUrl { get; private set; }
    }
}
