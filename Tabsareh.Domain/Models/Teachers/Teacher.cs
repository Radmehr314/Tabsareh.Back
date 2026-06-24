namespace Tabsareh.Domain.Models.Teachers
{
    public class Teacher : BaseEntity<string>
    {
        public Teacher(string firstName, string lastName, string? profileImage, string? description)
        {
            FirstName = firstName;
            LastName = lastName;
            ProfileImage = profileImage;
            Description = description;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void Update(string firstName, string lastName, string? profileImage, string? description)
        {
            FirstName = firstName;
            LastName = lastName;
            ProfileImage = profileImage;
            Description = description;
            UpdatedAt = DateTime.Now;
        }

        public void Delete() => IsDeleted = true;

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? ProfileImage { get; private set; }
        public string? Description { get; private set; }
        public bool IsDeleted { get; set; }
    }
}
