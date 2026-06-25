namespace Tabsareh.Domain.Models.Admins
{
    public class Admin : BaseEntity<string>
    {
        private Admin() { } 

        public Admin(string userName, string firstName, string lastName, string phone, string password, string salt, bool isBan, string? roleId)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Password = password;
            Salt = salt;
            IsBan = isBan;
            RoleId = roleId;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void Update(string userName, string firstName, string lastName, string phone, string? password, string? salt, string? roleId)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            if (!string.IsNullOrWhiteSpace(password))
            {
                Password = password;
                Salt = salt;
            }
            RoleId = roleId;
            UpdatedAt = DateTime.Now;
        }

        public void Ban() => IsBan = true;

        public void UnBan() => IsBan = false;

        public void Delete()
        {
            this.IsDeleted = true;
        }

        public string UserName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }
        public bool IsBan { get; private set; } = false;
        public bool IsDeleted { get; set; }
        public string? RoleId { get; private set; }
    }
}
