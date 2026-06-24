namespace Tabsareh.Domain.Common;

public class QueryOptions
{
    public int Skip { get; set; } = 0;
    public int Limit { get; set; } = 50;
    public string? SortBy { get; set; }
    public bool Descending { get; set; } = true;
}
