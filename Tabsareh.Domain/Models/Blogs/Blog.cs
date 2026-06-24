namespace Tabsareh.Domain.Models.Blogs
{
    public class Blog : BaseEntity<string>
    {
        private Blog() { }

        public Blog(
            string title,
            string slug,
            string? titleImage,
            string body,
            string? excerpt,
            string? categoryId,
            string? metaTitle,
            string? metaDescription,
            string? metaKeywords,
            bool isPublished)
        {
            Title = title;
            Slug = slug;
            TitleImage = titleImage;
            Body = body;
            Excerpt = excerpt;
            CategoryId = categoryId;
            MetaTitle = metaTitle;
            MetaDescription = metaDescription;
            MetaKeywords = metaKeywords;
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
            string? titleImage,
            string body,
            string? excerpt,
            string? categoryId,
            string? metaTitle,
            string? metaDescription,
            string? metaKeywords,
            bool isPublished)
        {
            Title = title;
            Slug = slug;
            TitleImage = titleImage;
            Body = body;
            Excerpt = excerpt;
            CategoryId = categoryId;
            MetaTitle = metaTitle;
            MetaDescription = metaDescription;
            MetaKeywords = metaKeywords;
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
        public string? TitleImage { get; private set; }
        public string Body { get; private set; }
        public string? Excerpt { get; private set; }
        public string? CategoryId { get; private set; }
        public string? MetaTitle { get; private set; }
        public string? MetaDescription { get; private set; }
        public string? MetaKeywords { get; private set; }
        public bool IsPublished { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public bool IsDeleted { get; set; }
    }
}
