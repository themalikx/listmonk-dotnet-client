# Listmonk.Client

A .NET SDK for the [listmonk](https://listmonk.app/) HTTP API.

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

## Package

```bash
dotnet pack src/Listmonk.Client/Listmonk.Client.csproj -c Release
```

## Notes

- The SDK follows the documented Listmonk response envelope shape where `data` contains the payload.
- JSON request bodies are serialized with snake_case field names to match the API.
- The public API surface is intentionally typed so the package can be consumed from older .NET Core and .NET Framework projects.
