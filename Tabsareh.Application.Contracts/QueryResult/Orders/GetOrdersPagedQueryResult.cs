namespace Tabsareh.Application.Contracts.QueryResult.Orders
{
    public class GetOrdersPagedQueryResult
    {
        public List<OrderItemResult> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
