namespace Tabsareh.Domain.Models.ContentOwners
{
    public class ContentOwner : BaseEntity<string>
    {
        private ContentOwner() { } // برای EF Core

        public ContentOwner(string name, string userName, string password, string salt, bool isBan)
        {
            Name = name;
            UserName = userName;
            Password = password;
            Salt = salt;
            IsBan = isBan;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void Update(string name, string userName, string? password, string? salt)
        {
            Name = name;
            UserName = userName;
            if (!string.IsNullOrWhiteSpace(password))
            {
                Password = password;
                Salt = salt;
            }
            UpdatedAt = DateTime.Now;
        }

        public void Ban() => IsBan = true;

        public void UnBan() => IsBan = false;

        public void Delete() => IsDeleted = true;

        public string Name { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }
        public bool IsBan { get; private set; } = false;
        public bool IsDeleted { get; set; }
    }
}
