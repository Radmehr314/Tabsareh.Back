namespace Tabsareh.Application.Contracts.QueryResult.ContentOwner
{
    public class ContentOwnerItemResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public bool IsBan { get; set; }
        public decimal DebtAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string CreatedAt { get; set; }
    }
}
