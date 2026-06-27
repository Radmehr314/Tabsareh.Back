using Tabsareh.Application.Contracts.QueryResult.Orders;

namespace Tabsareh.Application.Contracts.QueryResult.Cart
{
    public class CartResult
    {
        public List<CartItemResult> Items { get; set; } = new();
        public OrderInvoiceResult? Invoice { get; set; }
    }

    public class CartItemResult
    {
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string? BannerImage { get; set; }
        public decimal Price { get; set; }
        public decimal LicensePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? EffectiveDiscountPercent { get; set; }
    }
}
