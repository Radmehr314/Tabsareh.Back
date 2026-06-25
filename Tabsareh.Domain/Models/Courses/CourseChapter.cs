namespace Tabsareh.Domain.Models.Courses
{
    public class CourseChapter : BaseEntity<string>
    {
        private CourseChapter() { }

        public CourseChapter(string courseId, string title, string duration, int displayOrder)
        {
            CourseId = courseId;
            Title = title;
            Duration = duration;
            DisplayOrder = displayOrder;
        }

        public void SetId(string id) => Id = id;

        public void Update(string courseId, string title, string duration, int displayOrder)
        {
            CourseId = courseId;
            Title = title;
            Duration = duration;
            DisplayOrder = displayOrder;
            UpdatedAt = DateTime.Now;
        }

        public void Delete() => IsDeleted = true;

        public string CourseId { get; private set; }
        public string Title { get; private set; }
        public string Duration { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsDeleted { get; private set; }
        public List<CourseChapterVideo> Videos { get; private set; } = new();
    }
}
