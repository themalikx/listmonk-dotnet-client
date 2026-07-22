using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Listmonk.Client;

internal static class EmptyArray
{
    public static T[] Of<T>()
    {
        return new T[0];
    }
}

public sealed class ApiEnvelope<T>
{
    [JsonProperty("data")]
    public T? Data { get; set; }
}

public sealed class PaginatedResult<T>
{
    [JsonProperty("results")]
    public IReadOnlyList<T> Results { get; set; } = EmptyArray.Of<T>();

    [JsonProperty("query")]
    public string? Query { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("per_page")]
    public int PerPage { get; set; }

    [JsonProperty("page")]
    public int Page { get; set; }
}

public sealed class BoolData
{
    [JsonProperty("data")]
    public bool Data { get; set; }
}

public sealed class ListmonkList
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonProperty("uuid")]
    public string? Uuid { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("optin")]
    public string? OptIn { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("tags")]
    public IReadOnlyList<string> Tags { get; set; } = EmptyArray.Of<string>();

    [JsonProperty("subscriber_count")]
    public int? SubscriberCount { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }
}

public sealed class PublicListInfo
{
    [JsonProperty("uuid")]
    public string? Uuid { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}

public sealed class SubscriberListMembership
{
    [JsonProperty("subscription_status")]
    public string? SubscriptionStatus { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("uuid")]
    public string? Uuid { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("tags")]
    public IReadOnlyList<string> Tags { get; set; } = EmptyArray.Of<string>();

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
}

public sealed class Subscriber
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonProperty("uuid")]
    public string? Uuid { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("attribs")]
    public JObject Attribs { get; set; } = new JObject();

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("lists")]
    public IReadOnlyList<SubscriberListMembership> Lists { get; set; } = EmptyArray.Of<SubscriberListMembership>();
}

public sealed class SubscriberActivity
{
    [JsonProperty("campaign_views")]
    public IReadOnlyList<JObject> CampaignViews { get; set; } = EmptyArray.Of<JObject>();

    [JsonProperty("link_clicks")]
    public IReadOnlyList<JObject> LinkClicks { get; set; } = EmptyArray.Of<JObject>();
}

public sealed class SubscriberExport
{
    [JsonProperty("profile")]
    public IReadOnlyList<Subscriber> Profile { get; set; } = EmptyArray.Of<Subscriber>();

    [JsonProperty("subscriptions")]
    public IReadOnlyList<JObject> Subscriptions { get; set; } = EmptyArray.Of<JObject>();

    [JsonProperty("campaign_views")]
    public IReadOnlyList<JObject> CampaignViews { get; set; } = EmptyArray.Of<JObject>();

    [JsonProperty("link_clicks")]
    public IReadOnlyList<JObject> LinkClicks { get; set; } = EmptyArray.Of<JObject>();
}

public sealed class SubscriberBounce
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("source")]
    public string? Source { get; set; }

    [JsonProperty("meta")]
    public JObject Meta { get; set; } = new JObject();

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("subscriber_uuid")]
    public string? SubscriberUuid { get; set; }

    [JsonProperty("subscriber_id")]
    public int SubscriberId { get; set; }

    [JsonProperty("campaign")]
    public BounceCampaignRef? Campaign { get; set; }
}

public sealed class BounceCampaignRef
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}

public sealed class Campaign
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonProperty("views")]
    public int Views { get; set; }

    [JsonProperty("clicks")]
    public int Clicks { get; set; }

    [JsonProperty("bounces")]
    public int? Bounces { get; set; }

    [JsonProperty("lists")]
    public IReadOnlyList<CampaignListRef> Lists { get; set; } = EmptyArray.Of<CampaignListRef>();

    [JsonProperty("started_at")]
    public DateTimeOffset? StartedAt { get; set; }

    [JsonProperty("to_send")]
    public int ToSend { get; set; }

    [JsonProperty("sent")]
    public int Sent { get; set; }

    [JsonProperty("uuid")]
    public string? Uuid { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("subject")]
    public string? Subject { get; set; }

    [JsonProperty("from_email")]
    public string? FromEmail { get; set; }

    [JsonProperty("body")]
    public string? Body { get; set; }

    [JsonProperty("body_source")]
    public string? BodySource { get; set; }

    [JsonProperty("altbody")]
    public string? AltBody { get; set; }

    [JsonProperty("send_at")]
    public DateTimeOffset? SendAt { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("content_type")]
    public string? ContentType { get; set; }

    [JsonProperty("tags")]
    public IReadOnlyList<string> Tags { get; set; } = EmptyArray.Of<string>();

    [JsonProperty("template_id")]
    public int? TemplateId { get; set; }

    [JsonProperty("messenger")]
    public string? Messenger { get; set; }

    [JsonProperty("headers")]
    public JObject Headers { get; set; } = new JObject();

    [JsonProperty("attribs")]
    public JObject Attribs { get; set; } = new JObject();

    [JsonProperty("archive")]
    public bool? Archive { get; set; }

    [JsonProperty("archive_template_id")]
    public int? ArchiveTemplateId { get; set; }

    [JsonProperty("archive_meta")]
    public JObject ArchiveMeta { get; set; } = new JObject();

    [JsonProperty("archive_slug")]
    public string? ArchiveSlug { get; set; }
}

public sealed class CampaignListRef
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}

public sealed class CampaignPreview
{
    [JsonProperty("html")]
    public string? Html { get; set; }
}

public sealed class CampaignStat
{
    [JsonProperty("campaign_id")]
    public int CampaignId { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("timestamp")]
    public DateTimeOffset? Timestamp { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }
}

public sealed class Template
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("body")]
    public string? Body { get; set; }

    [JsonProperty("body_source")]
    public string? BodySource { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("subject")]
    public string? Subject { get; set; }

    [JsonProperty("is_default")]
    public bool IsDefault { get; set; }
}

public sealed class MediaItem
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("uuid")]
    public string? Uuid { get; set; }

    [JsonProperty("filename")]
    public string? Filename { get; set; }

    [JsonProperty("content_type")]
    public string? ContentType { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("thumb_url")]
    public string? ThumbUrl { get; set; }

    [JsonProperty("thumb_uri")]
    public string? ThumbUri { get; set; }

    [JsonProperty("uri")]
    public string? Uri { get; set; }

    [JsonProperty("provider")]
    public string? Provider { get; set; }

    [JsonProperty("meta")]
    public JObject Meta { get; set; } = new JObject();

    [JsonProperty("url")]
    public string? Url { get; set; }
}

public sealed class ImportStatus
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("imported")]
    public int Imported { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }
}

public sealed class TransactionalMessageRequest
{
    [JsonProperty("subscriber_email")]
    public string? SubscriberEmail { get; set; }

    [JsonProperty("subscriber_id")]
    public int? SubscriberId { get; set; }

    [JsonProperty("subscriber_emails")]
    public IReadOnlyList<string>? SubscriberEmails { get; set; }

    [JsonProperty("subscriber_ids")]
    public IReadOnlyList<int>? SubscriberIds { get; set; }

    [JsonProperty("subscriber_mode")]
    public string? SubscriberMode { get; set; }

    [JsonProperty("template_id")]
    public int TemplateId { get; set; }

    [JsonProperty("from_email")]
    public string? FromEmail { get; set; }

    [JsonProperty("subject")]
    public string? Subject { get; set; }

    [JsonProperty("data")]
    public JObject? Data { get; set; }

    [JsonProperty("headers")]
    public IReadOnlyList<JObject>? Headers { get; set; }

    [JsonProperty("messenger")]
    public string? Messenger { get; set; }

    [JsonProperty("content_type")]
    public string? ContentType { get; set; }

    [JsonProperty("altbody")]
    public string? AltBody { get; set; }
}
