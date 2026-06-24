namespace Tabsareh.Domain.Models.Roles
{
    public class Role : BaseEntity<string>
    {
        private Role() { } // برای EF Core

        public Role(string name, List<string> permissions)
        {
            Name = name;
            Permissions = permissions ?? new List<string>();
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void Update(string name, List<string> permissions)
        {
            Name = name;
            Permissions = permissions ?? new List<string>();
            UpdatedAt = DateTime.Now;
        }

        public string Name { get; private set; }
        public List<string> Permissions { get; private set; }
    }
}
