using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Listmonk.Client;

internal static class ApiUtilities
{
    public static string BuildQueryString(IEnumerable<KeyValuePair<string, string?>> values)
    {
        var builder = new StringBuilder();
        var first = true;
        foreach (var pair in values)
        {
            if (string.IsNullOrEmpty(pair.Value))
            {
                continue;
            }

            if (!first)
            {
                builder.Append('&');
            }

            first = false;
            builder.Append(Uri.EscapeDataString(pair.Key));
            builder.Append('=');
            builder.Append(Uri.EscapeDataString(pair.Value!));
        }

        return builder.ToString();
    }

    public static string BuildRepeatedQueryString(string key, IEnumerable<string> values)
    {
        return BuildQueryString(values.Select(value => new KeyValuePair<string, string?>(key, value)));
    }

    public static string BuildRepeatedQueryString(string key, IEnumerable<int> values)
    {
        return BuildQueryString(values.Select(value => new KeyValuePair<string, string?>(key, value.ToString(CultureInfo.InvariantCulture))));
    }

    public static string AppendQuery(string path, string? query)
    {
        return string.IsNullOrEmpty(query) ? path : path + "?" + query;
    }
}
