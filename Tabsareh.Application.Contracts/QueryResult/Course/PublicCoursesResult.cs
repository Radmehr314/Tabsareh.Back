namespace Tabsareh.Application.Contracts.QueryResult.Course
{
    public class PublicCoursesResult
    {
        public decimal LicensePrice { get; set; }
        public List<PublicCourseItem> Courses { get; set; } = new();
    }

    public class PublicCourseItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? BannerImage { get; set; }
        public int DurationMinutes { get; set; }
        public string? CategoryName { get; set; }
        public string? TeacherName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal LicensePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? EffectiveDiscountPercent { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public List<CourseVideoResult> SampleVideos { get; set; } = new();
        public List<CoursePdfResult> PdfFiles { get; set; } = new();
        public double? AverageRating { get; set; }
        public int CommentCount { get; set; }
    }
}
