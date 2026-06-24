namespace Tabsareh.Domain.Models.DynamicPages
{
    public class DynamicPage : BaseEntity<string>
    {
        private DynamicPage() { }

        public DynamicPage(
            string title,
            string slug,
            string body,
            string? metaTitle,
            string? metaDescription,
            string? metaKeywords,
            int displayOrder,
            bool isPublished)
        {
            Title = title;
            Slug = slug;
            Body = body;
            MetaTitle = metaTitle;
            MetaDescription = metaDescription;
            MetaKeywords = metaKeywords;
            DisplayOrder = displayOrder;
            IsPublished = isPublished;
            PublishedAt = isPublished ? DateTime.Now : null;
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
            string slug,
            string body,
            string? metaTitle,
            string? metaDescription,
            string? metaKeywords,
            int displayOrder,
            bool isPublished)
        {
            Title = title;
            Slug = slug;
            Body = body;
            MetaTitle = metaTitle;
            MetaDescription = metaDescription;
            MetaKeywords = metaKeywords;
            DisplayOrder = displayOrder;
            if (!IsPublished && isPublished)
                PublishedAt = DateTime.Now;
            if (!isPublished)
                PublishedAt = null;
            IsPublished = isPublished;
            UpdatedAt = DateTime.Now;
        }

        public void Delete() => IsDeleted = true;

        public string Title { get; private set; }
        public string Slug { get; private set; }
        public string Body { get; private set; }
        public string? MetaTitle { get; private set; }
        public string? MetaDescription { get; private set; }
        public string? MetaKeywords { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsPublished { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public bool IsDeleted { get; set; }
    }
}
