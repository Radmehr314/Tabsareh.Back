namespace Tabsareh.Application.Contracts.QueryResult.Dashboard
{
    public class DashboardStatsResult
    {
        public decimal TotalSalesAmount { get; set; }
        public decimal TodaySalesAmount { get; set; }
        public int PendingCardToCardCount { get; set; }
    }
}
