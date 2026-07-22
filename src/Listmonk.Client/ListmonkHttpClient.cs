using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Listmonk.Client;

public sealed class ListmonkHttpClient : IDisposable
{
    private readonly HttpClient httpClient;
    private readonly bool disposeHttpClient;
    private readonly JsonSerializerSettings serializerSettings;

    public ListmonkHttpClient(ListmonkClientOptions options)
        : this(new HttpClient(), options, true)
    {
    }

    public ListmonkHttpClient(HttpClient httpClient, ListmonkClientOptions options, bool disposeHttpClient = false)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        Options = options ?? throw new ArgumentNullException(nameof(options));
        this.disposeHttpClient = disposeHttpClient;
        serializerSettings = CreateSerializerSettings();

        this.httpClient.BaseAddress = options.BaseUrl;
        this.httpClient.Timeout = options.Timeout;

        if (!string.IsNullOrWhiteSpace(options.AuthorizationToken))
        {
            this.httpClient.DefaultRequestHeaders.Remove("Authorization");
            this.httpClient.DefaultRequestHeaders.Add("Authorization", options.AuthorizationToken);
        }
        else if (!string.IsNullOrWhiteSpace(options.Username) && !string.IsNullOrWhiteSpace(options.AccessToken))
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(options.Username + ":" + options.AccessToken));
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
        }

        foreach (var header in options.DefaultHeaders)
        {
            this.httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }

        Lists = new ListsApi(this);
        Subscribers = new SubscribersApi(this);
        Campaigns = new CampaignsApi(this);
        Templates = new TemplatesApi(this);
        Media = new MediaApi(this);
        Import = new ImportApi(this);
        Bounces = new BouncesApi(this);
        Transactional = new TransactionalApi(this);
    }

    public ListmonkClientOptions Options { get; }

    public ListsApi Lists { get; }

    public SubscribersApi Subscribers { get; }

    public CampaignsApi Campaigns { get; }

    public TemplatesApi Templates { get; }

    public MediaApi Media { get; }

    public ImportApi Import { get; }

    public BouncesApi Bounces { get; }

    public TransactionalApi Transactional { get; }

    public static ListmonkHttpClient Create(ListmonkClientOptions options)
    {
        return new ListmonkHttpClient(options);
    }

    public async Task<T> GetAsync<T>(string path, IDictionary<string, string>? query = null, CancellationToken cancellationToken = default)
    {
        using (var request = new HttpRequestMessage(HttpMethod.Get, BuildUri(path, query)))
        {
            return await SendAsync<T>(request, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<T> PostAsync<T>(string path, object? body = null, CancellationToken cancellationToken = default)
    {
        using (var request = new HttpRequestMessage(HttpMethod.Post, path))
        {
            if (body != null)
            {
                request.Content = CreateJsonContent(body);
            }

            return await SendAsync<T>(request, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<T> PutAsync<T>(string path, object? body = null, CancellationToken cancellationToken = default)
    {
        using (var request = new HttpRequestMessage(HttpMethod.Put, path))
        {
            if (body != null)
            {
                request.Content = CreateJsonContent(body);
            }

            return await SendAsync<T>(request, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<T> DeleteAsync<T>(string path, IDictionary<string, string>? query = null, CancellationToken cancellationToken = default)
    {
        using (var request = new HttpRequestMessage(HttpMethod.Delete, BuildUri(path, query)))
        {
            return await SendAsync<T>(request, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<T> PostMultipartAsync<T>(string path, MultipartFormDataContent content, CancellationToken cancellationToken = default)
    {
        using (var request = new HttpRequestMessage(HttpMethod.Post, path))
        {
            request.Content = content;
            return await SendAsync<T>(request, cancellationToken).ConfigureAwait(false);
        }
    }

    public string Serialize(object value)
    {
        return JsonConvert.SerializeObject(value, serializerSettings);
    }

    public T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, serializerSettings)!;
    }

    internal async Task<T> SendAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using (var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
        {
            var responseText = response.Content == null ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new ListmonkException("Listmonk API request failed.", response.StatusCode, responseText);
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)responseText;
            }

            if (string.IsNullOrWhiteSpace(responseText))
            {
                return default!;
            }

            return JsonConvert.DeserializeObject<T>(responseText, serializerSettings)!;
        }
    }

    internal Uri BuildUri(string path, IDictionary<string, string>? query)
    {
        var builder = new UriBuilder(new Uri(httpClient.BaseAddress!, path));
        if (query != null && query.Count > 0)
        {
            var first = true;
            var queryBuilder = new StringBuilder();
            foreach (var pair in query)
            {
                if (pair.Value == null)
                {
                    continue;
                }

                if (!first)
                {
                    queryBuilder.Append('&');
                }

                first = false;
                queryBuilder.Append(Uri.EscapeDataString(pair.Key));
                queryBuilder.Append('=');
                queryBuilder.Append(Uri.EscapeDataString(pair.Value));
            }

            builder.Query = queryBuilder.ToString();
        }

        return builder.Uri;
    }

    private static StringContent CreateJsonContent(object body)
    {
        var json = JsonConvert.SerializeObject(body, Formatting.None, CreateSerializerSettings());
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    internal static IDictionary<string, string> ToQuery(params KeyValuePair<string, string?>[] items)
    {
        var query = new Dictionary<string, string>();
        for (var index = 0; index < items.Length; index++)
        {
            var item = items[index];
            if (!string.IsNullOrEmpty(item.Value))
            {
                query[item.Key] = item.Value!;
            }
        }

        return query;
    }

    private static JsonSerializerSettings CreateSerializerSettings()
    {
        return new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            FloatParseHandling = FloatParseHandling.Double,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
        };
    }

    public void Dispose()
    {
        if (disposeHttpClient)
        {
            httpClient.Dispose();
        }
    }
}
