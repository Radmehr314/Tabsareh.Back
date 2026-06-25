namespace Tabsareh.Domain.Models.Courses
{
    public class Course : BaseEntity<string>
    {
        private Course() { }

        public Course(
            string title,
            string? bannerImage,
            int durationMinutes,
            string? categoryId,
            string teacherId,
            string contentOwnerId,
            string? description,
            decimal price,
            decimal settlementBasePrice,
            decimal contentOwnerSharePercent,
            bool isActive,
            decimal? discountPercent = null,
            DateTime? discountStartDate = null,
            DateTime? discountEndDate = null)
        {
            Title = title;
            BannerImage = bannerImage;
            DurationMinutes = durationMinutes;
            CategoryId = categoryId;
            TeacherId = teacherId;
            ContentOwnerId = contentOwnerId;
            Description = description;
            Price = price;
            SettlementBasePrice = settlementBasePrice;
            ContentOwnerSharePercent = contentOwnerSharePercent;
            IsActive = isActive;
            SetDiscount(discountPercent, discountStartDate, discountEndDate);
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void Update(
            string title,
            string? bannerImage,
            int durationMinutes,
            string? categoryId,
            string teacherId,
            string contentOwnerId,
            string? description,
            decimal price,
            decimal settlementBasePrice,
            decimal contentOwnerSharePercent,
            bool isActive,
            decimal? discountPercent = null,
            DateTime? discountStartDate = null,
            DateTime? discountEndDate = null)
        {
            Title = title;
            BannerImage = bannerImage;
            DurationMinutes = durationMinutes;
            CategoryId = categoryId;
            TeacherId = teacherId;
            ContentOwnerId = contentOwnerId;
            Description = description;
            Price = price;
            SettlementBasePrice = settlementBasePrice;
            ContentOwnerSharePercent = contentOwnerSharePercent;
            IsActive = isActive;
            SetDiscount(discountPercent, discountStartDate, discountEndDate);
            UpdatedAt = DateTime.Now;
        }

        public void Delete() => IsDeleted = true;

        private void SetDiscount(decimal? percent, DateTime? startDate, DateTime? endDate)
        {
            DiscountPercent = percent;
            DiscountStartDate = startDate?.Date;
            DiscountEndDate = endDate?.Date;
        }

        public string Title { get; private set; }
        public string? BannerImage { get; private set; }
        public int DurationMinutes { get; private set; }
        public string? CategoryId { get; private set; }
        public string TeacherId { get; private set; }
        public string ContentOwnerId { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public decimal SettlementBasePrice { get; private set; }
        public decimal ContentOwnerSharePercent { get; private set; }
        public decimal? DiscountPercent { get; private set; }
        public DateTime? DiscountStartDate { get; private set; }
        public DateTime? DiscountEndDate { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; set; }
        public List<CourseSampleVideo> SampleVideos { get; private set; } = new();
        public List<CoursePdfFile> PdfFiles { get; private set; } = new();
        public List<CourseChapter> Chapters { get; private set; } = new();
    }
}
