namespace Tabsareh.Domain.Models.Carts
{
    public class CartItem : BaseEntity<string>
    {
        private CartItem() { }

        public CartItem(string cartId, string courseId)
        {
            CartId = cartId;
            CourseId = courseId;
        }

        public string CartId { get; private set; }
        public string CourseId { get; private set; }

        public void SetId(string id) => Id = id;
    }
}
