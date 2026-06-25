namespace Tabsareh.Application.Contracts.QueryResult.Course
{
    public class CourseItemResult
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? BannerImage { get; set; }
        public int DurationMinutes { get; set; }
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string ContentOwnerId { get; set; }
        public string? ContentOwnerName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal SettlementBasePrice { get; set; }
        public decimal ContentOwnerSharePercent { get; set; }
        public decimal? DiscountPercent { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public string? DiscountStartDatePersian { get; set; }
        public string? DiscountEndDatePersian { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CourseVideoResult> SampleVideos { get; set; } = new();
        public List<CoursePdfResult> PdfFiles { get; set; } = new();
    }
}
