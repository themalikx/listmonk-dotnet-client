using System;
using System.Collections.Generic;

namespace Listmonk.Client;

public sealed class ListmonkClientOptions
{
    public Uri BaseUrl { get; set; } = new Uri("http://localhost:9000", UriKind.Absolute);

    public string? Username { get; set; }

    public string? AccessToken { get; set; }

    public string? AuthorizationToken { get; set; }

    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);

    public IDictionary<string, string> DefaultHeaders { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
}
