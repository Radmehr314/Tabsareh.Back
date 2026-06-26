namespace Tabsareh.Domain.Models.CourseComments
{
    public class CourseComment : BaseEntity<string>
    {
        private CourseComment() { }

        public CourseComment(
            string courseId,
            string authorName,
            string? authorPhone,
            string content,
            int rating)
        {
            CourseId = courseId;
            AuthorName = authorName;
            AuthorPhone = authorPhone;
            Content = content;
            Rating = rating;
            IsApproved = false;
            IsDeleted = false;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id)) throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void Approve()
        {
            IsApproved = true;
            UpdatedAt = DateTime.Now;
        }

        public void Reject()
        {
            IsApproved = false;
            IsDeleted = true;
            UpdatedAt = DateTime.Now;
        }

        public string CourseId { get; private set; }
        public string AuthorName { get; private set; }
        public string? AuthorPhone { get; private set; }
        public string Content { get; private set; }
        public int Rating { get; private set; }
        public bool IsApproved { get; private set; }
        public bool IsDeleted { get; private set; }
    }
}
