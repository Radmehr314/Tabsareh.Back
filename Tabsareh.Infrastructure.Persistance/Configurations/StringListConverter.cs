using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    /// <summary>
    /// تبدیل List&lt;string&gt; به/از JSON برای ذخیره در یک ستون nvarchar.
    /// </summary>
    public static class StringListConverter
    {
        public static readonly ValueConverter<List<string>, string> Converter =
            new(
                v => JsonSerializer.Serialize(v ?? new List<string>(), (JsonSerializerOptions?)null),
                v => string.IsNullOrWhiteSpace(v)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

        public static readonly ValueComparer<List<string>> Comparer =
            new(
                (a, b) => (a ?? new List<string>()).SequenceEqual(b ?? new List<string>()),
                v => v == null ? 0 : v.Aggregate(0, (acc, s) => HashCode.Combine(acc, s.GetHashCode())),
                v => v == null ? new List<string>() : v.ToList());
    }
}
