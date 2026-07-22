using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Listmonk.Client;

public sealed class ListsApi
{
    private readonly ListmonkHttpClient client;

    internal ListsApi(ListmonkHttpClient client)
    {
        this.client = client;
    }

    public async Task<PaginatedResult<ListmonkList>> GetListsAsync(string? query = null, string? status = null, bool minimal = false, IReadOnlyList<string>? tags = null, string? orderBy = null, string? order = null, int? page = null, string? perPage = null, CancellationToken cancellationToken = default)
    {
        var queryItems = new List<KeyValuePair<string, string?>>
        {
            new("query", query),
            new("status", status),
            new("minimal", minimal ? "true" : null),
            new("order_by", orderBy),
            new("order", order),
            new("page", page?.ToString()),
            new("per_page", perPage),
        };

        if (tags != null)
        {
            foreach (var tag in tags)
            {
                queryItems.Add(new KeyValuePair<string, string?>("tag", tag));
            }
        }

        var path = ApiUtilities.AppendQuery("/api/lists", ApiUtilities.BuildQueryString(queryItems));
        var envelope = await client.GetAsync<ApiEnvelope<PaginatedResult<ListmonkList>>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public Task<IReadOnlyList<PublicListInfo>> GetPublicListsAsync(CancellationToken cancellationToken = default)
    {
        return client.GetAsync<IReadOnlyList<PublicListInfo>>("/api/public/lists", null, cancellationToken);
    }

    public Task<ListmonkList> GetListAsync(int id, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<ListmonkList>($"/api/lists/{id}", cancellationToken);
    }

    public Task<ListmonkList> CreateListAsync(ListUpsertRequest request, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<ListmonkList>("/api/lists", cancellationToken, request, HttpMethod.Post);
    }

    public Task<ListmonkList> UpdateListAsync(int id, ListUpsertRequest request, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<ListmonkList>($"/api/lists/{id}", cancellationToken, request, HttpMethod.Put);
    }

    public Task<bool> DeleteListAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteEnvelopeAsync($"/api/lists/{id}", cancellationToken);
    }

    public Task<bool> DeleteListsAsync(IReadOnlyList<int> ids, string? query = null, CancellationToken cancellationToken = default)
    {
        var queryItems = new List<KeyValuePair<string, string?>>();
        if (query != null)
        {
            queryItems.Add(new KeyValuePair<string, string?>("query", query));
        }

        foreach (var id in ids)
        {
            queryItems.Add(new KeyValuePair<string, string?>("id", id.ToString()));
        }

        return DeleteEnvelopeAsync(ApiUtilities.AppendQuery("/api/lists", ApiUtilities.BuildQueryString(queryItems)), cancellationToken);
    }

    private async Task<T> GetEnvelopeAsync<T>(string path, CancellationToken cancellationToken, object? body = null, HttpMethod? method = null)
    {
        ApiEnvelope<T> envelope;
        if (method == HttpMethod.Post)
        {
            envelope = await client.PostAsync<ApiEnvelope<T>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else if (method == HttpMethod.Put)
        {
            envelope = await client.PutAsync<ApiEnvelope<T>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            envelope = await client.GetAsync<ApiEnvelope<T>>(path, null, cancellationToken).ConfigureAwait(false);
        }

        return envelope.Data!;
    }

    private async Task<bool> DeleteEnvelopeAsync(string path, CancellationToken cancellationToken)
    {
        var envelope = await client.DeleteAsync<ApiEnvelope<bool>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data;
    }
}

public sealed class SubscribersApi
{
    private readonly ListmonkHttpClient client;

    internal SubscribersApi(ListmonkHttpClient client)
    {
        this.client = client;
    }

    public async Task<PaginatedResult<Subscriber>> GetSubscribersAsync(string? query = null, IReadOnlyList<int>? listIds = null, string? subscriptionStatus = null, string? orderBy = null, string? order = null, int? page = null, string? perPage = null, CancellationToken cancellationToken = default)
    {
        var queryItems = new List<KeyValuePair<string, string?>>
        {
            new("query", query),
            new("subscription_status", subscriptionStatus),
            new("order_by", orderBy),
            new("order", order),
            new("page", page?.ToString()),
            new("per_page", perPage),
        };

        if (listIds != null)
        {
            foreach (var id in listIds)
            {
                queryItems.Add(new KeyValuePair<string, string?>("list_id", id.ToString()));
            }
        }

        var path = ApiUtilities.AppendQuery("/api/subscribers", ApiUtilities.BuildQueryString(queryItems));
        var envelope = await client.GetAsync<ApiEnvelope<PaginatedResult<Subscriber>>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public Task<Subscriber> GetSubscriberAsync(int id, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Subscriber>($"/api/subscribers/{id}", cancellationToken);
    }

    public async Task<SubscriberActivity> GetSubscriberActivityAsync(int id, CancellationToken cancellationToken = default)
    {
        var envelope = await client.GetAsync<ApiEnvelope<SubscriberActivity>>($"/api/subscribers/{id}/activity", null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public Task<SubscriberExport> GetSubscriberExportAsync(int id, CancellationToken cancellationToken = default)
    {
        return client.GetAsync<SubscriberExport>($"/api/subscribers/{id}/export", null, cancellationToken);
    }

    public async Task<IReadOnlyList<SubscriberBounce>> GetSubscriberBouncesAsync(int id, CancellationToken cancellationToken = default)
    {
        var envelope = await client.GetAsync<ApiEnvelope<IReadOnlyList<SubscriberBounce>>>($"/api/subscribers/{id}/bounces", null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public Task<Subscriber> CreateSubscriberAsync(SubscriberCreateRequest request, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Subscriber>("/api/subscribers", cancellationToken, request, HttpMethod.Post);
    }

    public Task<bool> SendOptInAsync(int id, CancellationToken cancellationToken = default)
    {
        return SendEnvelopeAsync($"/api/subscribers/{id}/optin", cancellationToken);
    }

    public Task<bool> PublicSubscriptionAsync(PublicSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        return SendEnvelopeAsync("/api/public/subscription", cancellationToken, request);
    }

    public async Task<bool> PublicSubscriptionFormAsync(string email, string? name, IReadOnlyList<string> listUuids, CancellationToken cancellationToken = default)
    {
        var form = new List<KeyValuePair<string, string>>
        {
            new("email", email)
        };

        if (!string.IsNullOrWhiteSpace(name))
        {
            form.Add(new KeyValuePair<string, string>("name", name!));
        }

        foreach (var listUuid in listUuids)
        {
            form.Add(new KeyValuePair<string, string>("l", listUuid));
        }

        using (var content = new FormUrlEncodedContent(form))
        using (var request = new HttpRequestMessage(HttpMethod.Post, "/api/public/subscription") { Content = content })
        {
            var envelope = await client.SendAsync<ApiEnvelope<bool>>(request, cancellationToken).ConfigureAwait(false);
            return envelope.Data;
        }
    }

    public Task<bool> ManageSubscriberListsAsync(SubscriberListMembershipRequest request, CancellationToken cancellationToken = default)
    {
        return SendEnvelopeAsync("/api/subscribers/lists", cancellationToken, request, HttpMethod.Put);
    }

    public Task<bool> ManageSubscriberListsByQueryAsync(SubscriberListMembershipRequest request, CancellationToken cancellationToken = default)
    {
        return SendEnvelopeAsync("/api/subscribers/query/lists", cancellationToken, request, HttpMethod.Put);
    }

    public Task<Subscriber> UpdateSubscriberAsync(int id, SubscriberUpdateRequest request, bool patch = false, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Subscriber>($"/api/subscribers/{id}", cancellationToken, request, patch ? new HttpMethod("PATCH") : HttpMethod.Put);
    }

    public Task<bool> BlocklistSubscriberAsync(int id, CancellationToken cancellationToken = default)
    {
        return SendEnvelopeAsync($"/api/subscribers/{id}/blocklist", cancellationToken, method: HttpMethod.Put);
    }

    public Task<bool> BlocklistSubscribersAsync(IReadOnlyList<int> ids, CancellationToken cancellationToken = default)
    {
        return SendEnvelopeAsync("/api/subscribers/blocklist", cancellationToken, new { ids }, HttpMethod.Put);
    }

    public Task<bool> BlocklistSubscribersByQueryAsync(SubscriberBlocklistRequest request, CancellationToken cancellationToken = default)
    {
        return SendEnvelopeAsync("/api/subscribers/query/blocklist", cancellationToken, request, HttpMethod.Put);
    }

    public Task<bool> DeleteSubscriberAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteEnvelopeAsync($"/api/subscribers/{id}", cancellationToken);
    }

    public Task<bool> DeleteSubscriberBouncesAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteEnvelopeAsync($"/api/subscribers/{id}/bounces", cancellationToken);
    }

    public Task<bool> DeleteSubscribersAsync(IReadOnlyList<int> ids, CancellationToken cancellationToken = default)
    {
        var queryItems = new List<KeyValuePair<string, string?>>();
        foreach (var id in ids)
        {
            queryItems.Add(new KeyValuePair<string, string?>("id", id.ToString()));
        }

        return DeleteEnvelopeAsync(ApiUtilities.AppendQuery("/api/subscribers", ApiUtilities.BuildQueryString(queryItems)), cancellationToken);
    }

    public Task<bool> DeleteSubscribersByQueryAsync(SubscriberDeleteQueryRequest request, CancellationToken cancellationToken = default)
    {
        return SendEnvelopeAsync("/api/subscribers/query/delete", cancellationToken, request, HttpMethod.Post);
    }

    private async Task<T> GetEnvelopeAsync<T>(string path, CancellationToken cancellationToken, object? body = null, HttpMethod? method = null)
    {
        ApiEnvelope<T> envelope;
        if (method == HttpMethod.Post)
        {
            envelope = await client.PostAsync<ApiEnvelope<T>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else if (method == HttpMethod.Put)
        {
            envelope = await client.PutAsync<ApiEnvelope<T>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else if (method != null && string.Equals(method.Method, "PATCH", StringComparison.OrdinalIgnoreCase))
        {
            using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), path))
            {
                if (body != null)
                {
                    request.Content = new StringContent(client.Serialize(body), System.Text.Encoding.UTF8, "application/json");
                }

                envelope = await client.SendAsync<ApiEnvelope<T>>(request, cancellationToken).ConfigureAwait(false);
            }
        }
        else
        {
            envelope = await client.GetAsync<ApiEnvelope<T>>(path, null, cancellationToken).ConfigureAwait(false);
        }

        return envelope.Data!;
    }

    private async Task<bool> SendEnvelopeAsync(string path, CancellationToken cancellationToken, object? body = null, HttpMethod? method = null)
    {
        ApiEnvelope<bool> envelope;
        if (method == HttpMethod.Post)
        {
            envelope = await client.PostAsync<ApiEnvelope<bool>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else if (method == HttpMethod.Put)
        {
            envelope = await client.PutAsync<ApiEnvelope<bool>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else if (method == HttpMethod.Delete)
        {
            envelope = await client.DeleteAsync<ApiEnvelope<bool>>(path, null, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            using (var request = new HttpRequestMessage(method ?? HttpMethod.Post, path))
            {
                if (body != null)
                {
                    request.Content = new StringContent(client.Serialize(body), System.Text.Encoding.UTF8, "application/json");
                }

                envelope = await client.SendAsync<ApiEnvelope<bool>>(request, cancellationToken).ConfigureAwait(false);
            }
        }

        return envelope.Data;
    }

    private async Task<bool> DeleteEnvelopeAsync(string path, CancellationToken cancellationToken)
    {
        var envelope = await client.DeleteAsync<ApiEnvelope<bool>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data;
    }

}

public sealed class CampaignsApi
{
    private readonly ListmonkHttpClient client;

    internal CampaignsApi(ListmonkHttpClient client)
    {
        this.client = client;
    }

    public async Task<PaginatedResult<Campaign>> GetCampaignsAsync(string? query = null, IReadOnlyList<string>? status = null, IReadOnlyList<string>? tags = null, string? orderBy = null, string? order = null, int? page = null, string? perPage = null, bool noBody = false, CancellationToken cancellationToken = default)
    {
        var queryItems = new List<KeyValuePair<string, string?>>
        {
            new("query", query),
            new("order_by", orderBy),
            new("order", order),
            new("page", page?.ToString()),
            new("per_page", perPage),
            new("no_body", noBody ? "true" : null),
        };

        if (status != null)
        {
            foreach (var item in status)
            {
                queryItems.Add(new KeyValuePair<string, string?>("status", item));
            }
        }

        if (tags != null)
        {
            foreach (var tag in tags)
            {
                queryItems.Add(new KeyValuePair<string, string?>("tags", tag));
            }
        }

        var envelope = await client.GetAsync<ApiEnvelope<PaginatedResult<Campaign>>>(ApiUtilities.AppendQuery("/api/campaigns", ApiUtilities.BuildQueryString(queryItems)), null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public Task<Campaign> GetCampaignAsync(int id, bool noBody = false, CancellationToken cancellationToken = default)
    {
        var path = ApiUtilities.AppendQuery($"/api/campaigns/{id}", noBody ? "no_body=true" : null);
        return GetEnvelopeAsync<Campaign>(path, cancellationToken);
    }

    public Task<string> PreviewCampaignAsync(int id, CancellationToken cancellationToken = default)
    {
        return client.GetAsync<string>($"/api/campaigns/{id}/preview", null, cancellationToken);
    }

    public async Task<IReadOnlyList<JObject>> GetRunningStatsAsync(IReadOnlyList<int> campaignIds, CancellationToken cancellationToken = default)
    {
        var path = ApiUtilities.AppendQuery("/api/campaigns/running/stats", ApiUtilities.BuildRepeatedQueryString("campaign_id", campaignIds));
        var envelope = await client.GetAsync<ApiEnvelope<IReadOnlyList<JObject>>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public async Task<IReadOnlyList<CampaignStat>> GetAnalyticsAsync(string type, IReadOnlyList<int> ids, string from, string to, CancellationToken cancellationToken = default)
    {
        var queryItems = new List<KeyValuePair<string, string?>>
        {
            new("from", from),
            new("to", to),
        };

        foreach (var id in ids)
        {
            queryItems.Add(new KeyValuePair<string, string?>("id", id.ToString()));
        }

        var path = ApiUtilities.AppendQuery($"/api/campaigns/analytics/{type}", ApiUtilities.BuildQueryString(queryItems));
        var envelope = await client.GetAsync<ApiEnvelope<IReadOnlyList<CampaignStat>>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public Task<Campaign> CreateCampaignAsync(CampaignUpsertRequest request, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Campaign>("/api/campaigns", cancellationToken, request, HttpMethod.Post);
    }

    public Task<bool> TestCampaignAsync(int id, CampaignUpsertRequest request, CancellationToken cancellationToken = default)
    {
        return SendEnvelopeAsync($"/api/campaigns/{id}/test", cancellationToken, request, HttpMethod.Post);
    }

    public Task<Campaign> UpdateCampaignAsync(int id, CampaignUpsertRequest request, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Campaign>($"/api/campaigns/{id}", cancellationToken, request, HttpMethod.Put);
    }

    public Task<Campaign> UpdateCampaignStatusAsync(int id, string status, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Campaign>($"/api/campaigns/{id}/status", cancellationToken, new CampaignStatusRequest { Status = status }, HttpMethod.Put);
    }

    public Task<Campaign> UpdateCampaignArchiveAsync(int id, CampaignArchiveRequest request, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Campaign>($"/api/campaigns/{id}/archive", cancellationToken, request, HttpMethod.Put);
    }

    public Task<bool> DeleteCampaignAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteEnvelopeAsync($"/api/campaigns/{id}", cancellationToken);
    }

    public Task<bool> DeleteCampaignsAsync(IReadOnlyList<int> ids, string? query = null, CancellationToken cancellationToken = default)
    {
        var queryItems = new List<KeyValuePair<string, string?>>();
        if (query != null)
        {
            queryItems.Add(new KeyValuePair<string, string?>("query", query));
        }

        foreach (var id in ids)
        {
            queryItems.Add(new KeyValuePair<string, string?>("id", id.ToString()));
        }

        return DeleteEnvelopeAsync(ApiUtilities.AppendQuery("/api/campaigns", ApiUtilities.BuildQueryString(queryItems)), cancellationToken);
    }

    private async Task<T> GetEnvelopeAsync<T>(string path, CancellationToken cancellationToken, object? body = null, HttpMethod? method = null)
    {
        ApiEnvelope<T> envelope;
        if (method == HttpMethod.Post)
        {
            envelope = await client.PostAsync<ApiEnvelope<T>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else if (method == HttpMethod.Put)
        {
            envelope = await client.PutAsync<ApiEnvelope<T>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            envelope = await client.GetAsync<ApiEnvelope<T>>(path, null, cancellationToken).ConfigureAwait(false);
        }

        return envelope.Data!;
    }

    private async Task<bool> SendEnvelopeAsync(string path, CancellationToken cancellationToken, object? body = null, HttpMethod? method = null)
    {
        ApiEnvelope<bool> envelope;
        if (method == HttpMethod.Post)
        {
            envelope = await client.PostAsync<ApiEnvelope<bool>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else if (method == HttpMethod.Put)
        {
            envelope = await client.PutAsync<ApiEnvelope<bool>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            using (var request = new HttpRequestMessage(method ?? HttpMethod.Post, path))
            {
                if (body != null)
                {
                    request.Content = new StringContent(client.Serialize(body), System.Text.Encoding.UTF8, "application/json");
                }

                envelope = await client.SendAsync<ApiEnvelope<bool>>(request, cancellationToken).ConfigureAwait(false);
            }
        }

        return envelope.Data;
    }

    private async Task<bool> DeleteEnvelopeAsync(string path, CancellationToken cancellationToken)
    {
        var envelope = await client.DeleteAsync<ApiEnvelope<bool>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data;
    }
}

public sealed class TemplatesApi
{
    private readonly ListmonkHttpClient client;

    internal TemplatesApi(ListmonkHttpClient client)
    {
        this.client = client;
    }

    public async Task<IReadOnlyList<Template>> GetTemplatesAsync(CancellationToken cancellationToken = default)
    {
        var envelope = await client.GetAsync<ApiEnvelope<IReadOnlyList<Template>>>("/api/templates", null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public Task<Template> GetTemplateAsync(int id, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Template>($"/api/templates/{id}", cancellationToken);
    }

    public Task<string> PreviewTemplateAsync(int id, CancellationToken cancellationToken = default)
    {
        return client.GetAsync<string>($"/api/templates/{id}/preview", null, cancellationToken);
    }

    public Task<string> PreviewTemplateBodyAsync(TemplateUpsertRequest request, CancellationToken cancellationToken = default)
    {
        return client.PostAsync<string>("/api/templates/preview", request, cancellationToken);
    }

    public Task<Template> CreateTemplateAsync(TemplateUpsertRequest request, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Template>("/api/templates", cancellationToken, request, HttpMethod.Post);
    }

    public Task<Template> UpdateTemplateAsync(int id, TemplateUpsertRequest request, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Template>($"/api/templates/{id}", cancellationToken, request, HttpMethod.Put);
    }

    public Task<Template> SetDefaultTemplateAsync(int id, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<Template>($"/api/templates/{id}/default", cancellationToken, null, HttpMethod.Put);
    }

    public Task<bool> DeleteTemplateAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteEnvelopeAsync($"/api/templates/{id}", cancellationToken);
    }

    private async Task<T> GetEnvelopeAsync<T>(string path, CancellationToken cancellationToken, object? body = null, HttpMethod? method = null)
    {
        ApiEnvelope<T> envelope;
        if (method == HttpMethod.Post)
        {
            envelope = await client.PostAsync<ApiEnvelope<T>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else if (method == HttpMethod.Put)
        {
            envelope = await client.PutAsync<ApiEnvelope<T>>(path, body, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            envelope = await client.GetAsync<ApiEnvelope<T>>(path, null, cancellationToken).ConfigureAwait(false);
        }

        return envelope.Data!;
    }

    private async Task<bool> DeleteEnvelopeAsync(string path, CancellationToken cancellationToken)
    {
        var envelope = await client.DeleteAsync<ApiEnvelope<bool>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data;
    }
}

public sealed class MediaApi
{
    private readonly ListmonkHttpClient client;

    internal MediaApi(ListmonkHttpClient client)
    {
        this.client = client;
    }

    public async Task<IReadOnlyList<MediaItem>> GetMediaAsync(CancellationToken cancellationToken = default)
    {
        var envelope = await client.GetAsync<ApiEnvelope<IReadOnlyList<MediaItem>>>("/api/media", null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public Task<MediaItem> GetMediaAsync(int id, CancellationToken cancellationToken = default)
    {
        return GetEnvelopeAsync<MediaItem>($"/api/media/{id}", cancellationToken);
    }

    public async Task<MediaItem> UploadMediaAsync(string filePath, CancellationToken cancellationToken = default)
    {
        using (var content = new MultipartFormDataContent())
        using (var fileStream = File.OpenRead(filePath))
        using (var streamContent = new StreamContent(fileStream))
        {
            content.Add(streamContent, "file", Path.GetFileName(filePath));
            return await PostMultipartEnvelopeAsync<MediaItem>("/api/media", content, cancellationToken).ConfigureAwait(false);
        }
    }

    public Task<bool> DeleteMediaAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteEnvelopeAsync($"/api/media/{id}", cancellationToken);
    }

    private async Task<T> GetEnvelopeAsync<T>(string path, CancellationToken cancellationToken)
    {
        var envelope = await client.GetAsync<ApiEnvelope<T>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    private async Task<T> PostMultipartEnvelopeAsync<T>(string path, MultipartFormDataContent content, CancellationToken cancellationToken)
    {
        var envelope = await client.PostMultipartAsync<ApiEnvelope<T>>(path, content, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    private async Task<bool> DeleteEnvelopeAsync(string path, CancellationToken cancellationToken)
    {
        var envelope = await client.DeleteAsync<ApiEnvelope<bool>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data;
    }
}

public sealed class ImportApi
{
    private readonly ListmonkHttpClient client;

    internal ImportApi(ListmonkHttpClient client)
    {
        this.client = client;
    }

    public async Task<ImportStatus> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        var envelope = await client.GetAsync<ApiEnvelope<ImportStatus>>("/api/import/subscribers", null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public Task<string> GetLogsAsync(CancellationToken cancellationToken = default)
    {
        return client.GetAsync<string>("/api/import/subscribers/logs", null, cancellationToken);
    }

    public async Task<ImportStatus> ImportSubscribersAsync(string filePath, ImportSubscribersRequest request, CancellationToken cancellationToken = default)
    {
        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(client.Serialize(request)), "params");
            var stream = File.OpenRead(filePath);
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
            content.Add(fileContent, "file", Path.GetFileName(filePath));
            return await PostMultipartEnvelopeAsync<ImportStatus>("/api/import/subscribers", content, stream, cancellationToken).ConfigureAwait(false);
        }
    }

    public Task<ImportStatus> StopImportAsync(CancellationToken cancellationToken = default)
    {
        return DeleteEnvelopeAsync<ImportStatus>("/api/import/subscribers", cancellationToken);
    }

    private async Task<T> GetEnvelopeAsync<T>(string path, CancellationToken cancellationToken)
    {
        var envelope = await client.GetAsync<ApiEnvelope<T>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    private async Task<T> PostMultipartEnvelopeAsync<T>(string path, MultipartFormDataContent content, Stream streamToDispose, CancellationToken cancellationToken)
    {
        try
        {
            var envelope = await client.PostMultipartAsync<ApiEnvelope<T>>(path, content, cancellationToken).ConfigureAwait(false);
            return envelope.Data!;
        }
        finally
        {
            streamToDispose.Dispose();
            content.Dispose();
        }
    }

    private async Task<T> DeleteEnvelopeAsync<T>(string path, CancellationToken cancellationToken)
    {
        var envelope = await client.DeleteAsync<ApiEnvelope<T>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }
}

public sealed class BouncesApi
{
    private readonly ListmonkHttpClient client;

    internal BouncesApi(ListmonkHttpClient client)
    {
        this.client = client;
    }

    public async Task<PaginatedResult<SubscriberBounce>> GetBouncesAsync(IReadOnlyList<int>? campaignIds = null, int? page = null, string? perPage = null, string? source = null, string? orderBy = null, string? order = null, CancellationToken cancellationToken = default)
    {
        var queryItems = new List<KeyValuePair<string, string?>>
        {
            new("page", page?.ToString()),
            new("per_page", perPage),
            new("source", source),
            new("order_by", orderBy),
            new("order", order),
        };

        if (campaignIds != null)
        {
            foreach (var campaignId in campaignIds)
            {
                queryItems.Add(new KeyValuePair<string, string?>("campaign_id", campaignId.ToString()));
            }
        }

        var path = ApiUtilities.AppendQuery("/api/bounces", ApiUtilities.BuildQueryString(queryItems));
        var envelope = await client.GetAsync<ApiEnvelope<PaginatedResult<SubscriberBounce>>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data!;
    }

    public Task<bool> DeleteAllOrSelectedAsync(IReadOnlyList<int>? ids = null, bool? all = null, CancellationToken cancellationToken = default)
    {
        var queryItems = new List<KeyValuePair<string, string?>>();
        if (all.HasValue)
        {
            queryItems.Add(new KeyValuePair<string, string?>("all", all.Value ? "true" : "false"));
        }

        if (ids != null)
        {
            foreach (var id in ids)
            {
                queryItems.Add(new KeyValuePair<string, string?>("id", id.ToString()));
            }
        }

        return DeleteEnvelopeAsync(ApiUtilities.AppendQuery("/api/bounces", ApiUtilities.BuildQueryString(queryItems)), cancellationToken);
    }

    public Task<bool> DeleteBounceAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteEnvelopeAsync($"/api/bounces/{id}", cancellationToken);
    }

    public Task<bool> BlocklistBouncedSubscribersAsync(CancellationToken cancellationToken = default)
    {
        return PutEnvelopeAsync("/api/bounces/blocklist", cancellationToken);
    }

    private async Task<bool> PutEnvelopeAsync(string path, CancellationToken cancellationToken)
    {
        var envelope = await client.PutAsync<ApiEnvelope<bool>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data;
    }

    private async Task<bool> DeleteEnvelopeAsync(string path, CancellationToken cancellationToken)
    {
        var envelope = await client.DeleteAsync<ApiEnvelope<bool>>(path, null, cancellationToken).ConfigureAwait(false);
        return envelope.Data;
    }
}

public sealed class TransactionalApi
{
    private readonly ListmonkHttpClient client;

    internal TransactionalApi(ListmonkHttpClient client)
    {
        this.client = client;
    }

    public async Task<bool> SendAsync(TransactionalMessageRequest request, IEnumerable<string>? attachmentFilePaths = null, CancellationToken cancellationToken = default)
    {
        if (attachmentFilePaths == null)
        {
            var envelope = await client.PostAsync<ApiEnvelope<bool>>("/api/tx", request, cancellationToken).ConfigureAwait(false);
            return envelope.Data;
        }

        var content = new MultipartFormDataContent();
        content.Add(new StringContent(client.Serialize(request)), "data");

        var streams = new List<FileStream>();
        try
        {
            foreach (var filePath in attachmentFilePaths)
            {
                var stream = File.OpenRead(filePath);
                streams.Add(stream);
                var streamContent = new StreamContent(stream);
                content.Add(streamContent, "file", Path.GetFileName(filePath));
            }

            var envelope = await client.PostMultipartAsync<ApiEnvelope<bool>>("/api/tx", content, cancellationToken).ConfigureAwait(false);
            return envelope.Data;
        }
        finally
        {
            foreach (var stream in streams)
            {
                stream.Dispose();
            }
            content.Dispose();
        }
    }
}
