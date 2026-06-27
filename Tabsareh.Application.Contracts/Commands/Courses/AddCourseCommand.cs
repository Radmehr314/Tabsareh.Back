using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Courses
{
    public class AddCourseCommand : ICommand
    {
        public string Title { get; set; }
        public string? BannerImage { get; set; }
        public int DurationMinutes { get; set; }
        public string? CategoryId { get; set; }
        public string TeacherId { get; set; }
        public string ContentOwnerId { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal ContentOwnerSharePercent { get; set; }
        public decimal? DiscountPercent { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public List<CourseVideoCommandItem> SampleVideos { get; set; } = new();
        public List<CoursePdfCommandItem> PdfFiles { get; set; } = new();
    }
}
