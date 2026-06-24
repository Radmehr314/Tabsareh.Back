namespace Tabsareh.Domain.Models.Users
{
    public class User : BaseEntity<string>
    {
        private User() { }

        public User(string firstName, string lastName, string userName, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Phone = phone;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void Update(string firstName, string lastName, string userName, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Phone = phone;
            UpdatedAt = DateTime.Now;
        }

        public void Delete() => IsDeleted = true;

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserName { get; private set; }
        public string Phone { get; private set; }
        public bool IsDeleted { get; set; }
    }
}
