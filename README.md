# Listmonk.Client

<p align="left">
  <img src="https://img.shields.io/nuget/v/Listmonk.Client.svg" alt="NuGet" />
  <img src="https://img.shields.io/badge/.NET-Standard%201.3%20%7C%201.6%20%7C%202.0%20%7C%202.1%20%7C%20Framework%20%7C%206%20%7C%208-blue" alt=".NET targets" />
  <img src="https://img.shields.io/badge/license-AGPL--3.0-blueviolet" alt="License" />
</p>

An opinionated, strongly typed .NET SDK for the [listmonk](https://listmonk.app/) HTTP API.

It is designed to be easy to use from modern .NET applications while still supporting older .NET Core and .NET Framework targets.

## Highlights

- Multi-targeted for broad .NET compatibility.
- Typed client surface for core listmonk API areas.
- Basic Auth and `Authorization: token ...` support.
- JSON envelope handling that matches listmonk responses.
- Multipart support for media upload, import, and transactional attachments.

## Supported targets

This library targets:

- `netstandard1.3`
- `netstandard1.6`
- `netstandard2.0`
- `netstandard2.1`
- `net45`
- `net461`
- `net462`
- `net472`
- `net48`
- `net6.0`
- `net8.0`

## Install

After publishing to NuGet.org:

```bash
dotnet add package Listmonk.Client
```

## Authentication

Listmonk supports either Basic Auth or the `Authorization: token ...` header.

```csharp
var client = new ListmonkHttpClient(new ListmonkClientOptions
{
    BaseUrl = new Uri("https://listmonk.example.com"),
    Username = "api_user",
    AccessToken = "api_token"
});
```

Or:

```csharp
var client = new ListmonkHttpClient(new ListmonkClientOptions
{
    BaseUrl = new Uri("https://listmonk.example.com"),
    AuthorizationToken = "token api_user:api_token"
});
```

## Quick start

```csharp
using Listmonk.Client;

var client = new ListmonkHttpClient(new ListmonkClientOptions
{
    BaseUrl = new Uri("http://localhost:9000"),
    Username = "api_user",
    AccessToken = "token"
});

var lists = await client.Lists.GetListsAsync(perPage: "100");
var subscribers = await client.Subscribers.GetSubscribersAsync(page: 1, perPage: "20");
var campaigns = await client.Campaigns.GetCampaignsAsync(page: 1, perPage: "20");
```

## Implemented areas

- Lists
- Subscribers
- Campaigns
- Templates
- Media
- Import
- Bounces
- Transactional messages

## Build

```bash
dotnet build Listmonk.Client.sln -c Release
```

## Test

```bash
dotnet test Listmonk.Client.sln -c Release
```

## Contributing

Contributions are welcome.

Before opening a pull request:

1. Create a feature branch from `main`.
2. Keep the SDK surface typed and backwards compatible where possible.
3. Run `dotnet build` and `dotnet test` before submitting.
4. Include or update tests for any behavior change.
5. Update documentation when adding or changing public APIs.

Suggested pull request checklist:

- [ ] Code builds successfully
- [ ] Tests pass
- [ ] Documentation updated
- [ ] Package version updated if needed

## Contributors

- Malik Qasim - creator and maintainer

## Notes

- The SDK follows the documented listmonk response envelope shape where `data` contains the payload.
- JSON request bodies are serialized with snake_case field names to match the API.
- The public API surface is intentionally typed so the package can be consumed from older .NET Core and .NET Framework projects.
