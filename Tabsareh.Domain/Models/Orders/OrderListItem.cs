namespace Tabsareh.Domain.Models.Orders
{
    public class OrderListItem
    {
        public Order Order { get; set; }
        public string UserPhone { get; set; }
        public string? UserFullName { get; set; }
        public string CourseTitle { get; set; }
    }
}
