using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Listmonk.Client;

public sealed class ListUpsertRequest
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? OptIn { get; set; }
    public string? Status { get; set; }
    public IReadOnlyList<string>? Tags { get; set; }
    public string? Description { get; set; }
}

public sealed class SubscriberCreateRequest
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public IReadOnlyList<int>? Lists { get; set; }
    public JObject? Attribs { get; set; }
    public bool? PreconfirmSubscriptions { get; set; }
}

public sealed class SubscriberUpdateRequest
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public IReadOnlyList<int>? Lists { get; set; }
    public JObject? Attribs { get; set; }
    public bool? PreconfirmSubscriptions { get; set; }
}

public sealed class SubscriberListMembershipRequest
{
    public IReadOnlyList<int>? Ids { get; set; }
    public string? Action { get; set; }
    public IReadOnlyList<int>? TargetListIds { get; set; }
    public string? Status { get; set; }
    public string? Query { get; set; }
    public string? Search { get; set; }
    public IReadOnlyList<int>? ListIds { get; set; }
    public string? SubscriptionStatus { get; set; }
}

public sealed class SubscriberBlocklistRequest
{
    public IReadOnlyList<int>? Ids { get; set; }
    public string? Query { get; set; }
    public IReadOnlyList<int>? ListIds { get; set; }
}

public sealed class SubscriberDeleteQueryRequest
{
    public string? Query { get; set; }
    public IReadOnlyList<int>? ListIds { get; set; }
    public bool? All { get; set; }
}

public sealed class PublicSubscriptionRequest
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public IReadOnlyList<string>? ListUuids { get; set; }
}

public sealed class CampaignUpsertRequest
{
    public string? Name { get; set; }
    public string? Subject { get; set; }
    public IReadOnlyList<int>? Lists { get; set; }
    public string? FromEmail { get; set; }
    public string? Type { get; set; }
    public string? ContentType { get; set; }
    public string? Body { get; set; }
    public string? BodySource { get; set; }
    public string? AltBody { get; set; }
    public string? SendAt { get; set; }
    public string? Messenger { get; set; }
    public int? TemplateId { get; set; }
    public IReadOnlyList<string>? Tags { get; set; }
    public JObject? Headers { get; set; }
    public JObject? Attribs { get; set; }
    public IReadOnlyList<string>? Subscribers { get; set; }
}

public sealed class CampaignStatusRequest
{
    public string? Status { get; set; }
}

public sealed class CampaignArchiveRequest
{
    public bool Archive { get; set; }
    public int? ArchiveTemplateId { get; set; }
    public JObject? ArchiveMeta { get; set; }
    public string? ArchiveSlug { get; set; }
}

public sealed class TemplateUpsertRequest
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Subject { get; set; }
    public string? BodySource { get; set; }
    public string? Body { get; set; }
}

public sealed class ImportSubscribersRequest
{
    public string? Mode { get; set; }
    public string? Delim { get; set; }
    public IReadOnlyList<int>? Lists { get; set; }
    public bool? Overwrite { get; set; }
    public string? SubscriptionStatus { get; set; }
}
