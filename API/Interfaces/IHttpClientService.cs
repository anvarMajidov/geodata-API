namespace API.Interfaces;

public interface IHttpClientService
{
    Task<T> GetAsync<T>(string url);
}
