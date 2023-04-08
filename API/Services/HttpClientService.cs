using System.Net;
using API.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly int _timeoutSeconds;

    public HttpClientService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CoordinatesApi/1.0");
        _timeoutSeconds = config.GetValue("HttpClientTimeoutSeconds", defaultValue: 10);
    }

    public async Task<T> GetAsync<T>(string url)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_timeoutSeconds));
            
            var response = await _httpClient.GetAsync(url, cts.Token);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get data from {url}", null, response.StatusCode);
            }
            
            var settings = new JsonSerializerSettings 
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
        catch (OperationCanceledException)
        {
            throw new HttpRequestException("Request time exceeded", null, HttpStatusCode.RequestTimeout);
        }
    }
}
