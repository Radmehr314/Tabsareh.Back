namespace Tabsareh.Domain.Models.Categories
{
    public class Category : BaseEntity<string>
    {
        private Category() { }

        public Category(string name, string? parentId)
        {
            Name = name;
            ParentId = parentId;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void Update(string name, string? parentId)
        {
            Name = name;
            ParentId = parentId;
            UpdatedAt = DateTime.Now;
        }

        public void Delete() => IsDeleted = true;

        public string Name { get; private set; }
        public string? ParentId { get; private set; }
        public bool IsDeleted { get; set; }
    }
}
