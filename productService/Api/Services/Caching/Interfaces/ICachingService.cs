namespace productService.Api.Services.Caching.Interfaces;

public interface ICachingService
{
    Task<string> Get(string key);
    Task Set(string key, string value);
}
