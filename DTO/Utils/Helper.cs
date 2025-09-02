using System.Reflection;
using System.Text.Json;

namespace AuditTrailAPI.DTO.Utils
{
    public class Helper
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static string BuildChangesJson<T>(T? before, T? after, params string[] ignoreProps)
        {
            var ignore = new HashSet<string>(ignoreProps ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

            var dict = new Dictionary<string, object?>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(p => p.GetMethod != null && p.SetMethod != null);

            foreach (var p in props)
            {
                if (ignore.Contains(p.Name)) continue;

                var b = before is null ? null : p.GetValue(before);
                var a = after is null ? null : p.GetValue(after);

                var equal = (b?.ToString() ?? "") == (a?.ToString() ?? "");
                if (!equal)
                {
                    dict[p.Name] = new { before = b, after = a };
                }
            }

            return JsonSerializer.Serialize(dict, JsonOptions);
        }
    }
}
