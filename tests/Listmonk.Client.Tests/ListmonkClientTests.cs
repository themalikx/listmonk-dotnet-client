using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace Listmonk.Client.Tests;

public sealed class ListmonkClientTests
{
    [Fact]
    public async Task AddsBasicAuthAndParsesEnvelope()
    {
        HttpRequestMessage? capturedRequest = null;
        var handler = new TestHandler(request =>
        {
            capturedRequest = request;
            var json = JsonConvert.SerializeObject(new ApiEnvelope<ListmonkList>
            {
                Data = new ListmonkList { Id = 7, Name = "Alpha" }
            });

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        });

        using var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:9000") };
        var client = new ListmonkHttpClient(httpClient, new ListmonkClientOptions
        {
            BaseUrl = new Uri("http://localhost:9000"),
            Username = "api_user",
            AccessToken = "token"
        });

        var result = await client.Lists.GetListAsync(7);

        Assert.NotNull(capturedRequest);
        Assert.Equal(HttpMethod.Get, capturedRequest!.Method);
        Assert.Equal("/api/lists/7", capturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(capturedRequest.Headers.Authorization);
        Assert.Equal("Basic", capturedRequest.Headers.Authorization!.Scheme);
        Assert.Equal(7, result.Id);
        Assert.Equal("Alpha", result.Name);
    }

    [Fact]
    public async Task SendsJsonBodyForCreateCampaign()
    {
        HttpRequestMessage? capturedRequest = null;
        string? capturedBody = null;
        var handler = new TestHandler(request =>
        {
            capturedRequest = request;
            capturedBody = request.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
            var json = JsonConvert.SerializeObject(new ApiEnvelope<Campaign>
            {
                Data = new Campaign { Id = 11, Name = "Spring" }
            });

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        });

        using var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:9000") };
        var client = new ListmonkHttpClient(httpClient, new ListmonkClientOptions { BaseUrl = new Uri("http://localhost:9000") });

        var created = await client.Campaigns.CreateCampaignAsync(new CampaignUpsertRequest
        {
            Name = "Spring",
            Subject = "Hello",
            Lists = new[] { 1 },
            Type = "regular",
            ContentType = "richtext",
            Body = "<p>Hello</p>"
        });

        var request = capturedRequest;
        Assert.NotNull(request);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("/api/campaigns", request.RequestUri!.AbsolutePath);
        Assert.NotNull(capturedBody);
        var body = capturedBody;
        Assert.Contains("\"name\":\"Spring\"", body);
        Assert.Contains("\"content_type\":\"richtext\"", body);
        Assert.Equal(11, created.Id);
    }

    [Fact]
    public void SerializesSnakeCaseForRequests()
    {
        var client = new ListmonkHttpClient(new ListmonkClientOptions { BaseUrl = new Uri("http://localhost:9000") });

        var json = client.Serialize(new SubscriberCreateRequest
        {
            Email = "demo@example.com",
            PreconfirmSubscriptions = true
        });

        Assert.Contains("\"email\":", json);
        Assert.Contains("\"preconfirm_subscriptions\":true", json);
    }

    private sealed class TestHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> responder;

        public TestHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
        {
            this.responder = responder;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(responder(request));
        }
    }
}
