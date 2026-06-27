namespace Tabsareh.Domain.Models.Carts
{
    public class Cart : BaseEntity<string>
    {
        private Cart() { }

        public Cart(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }
        public List<CartItem> Items { get; private set; } = new();

        public void AddItem(string courseId)
        {
            if (Items.Any(x => x.CourseId == courseId)) return;
            Items.Add(new CartItem(Id, courseId));
            UpdatedAt = DateTime.Now;
        }

        public void RemoveItem(string courseId)
        {
            var item = Items.FirstOrDefault(x => x.CourseId == courseId);
            if (item is not null)
            {
                Items.Remove(item);
                UpdatedAt = DateTime.Now;
            }
        }

        public void Clear()
        {
            Items.Clear();
            UpdatedAt = DateTime.Now;
        }

        public void SetId(string id)
        {
            if (!string.IsNullOrWhiteSpace(Id)) throw new InvalidOperationException("Id is already set.");
            Id = id;
        }
    }
}
