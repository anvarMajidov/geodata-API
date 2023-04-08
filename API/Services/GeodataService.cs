using API.DTOs;
using API.Interfaces;
using Dadata;
using Mapster;

namespace API.Services;

public class GeodataService : IGeodataService
{
    private readonly string _token;
    private readonly string _geoCodingUrl;
    private readonly IHttpClientService _httpService;

    public GeodataService(IHttpClientService httpService, IConfiguration config)
    {
        _httpService = httpService;
        _token = config.GetValue<string>("Token");
        _geoCodingUrl = config.GetValue<string>("GeoCodingUrl");
    }
    
    /// <summary>
    /// Finds geo data for location by given query
    /// </summary>
    /// <param name="query">Parameters should be separated by comma. Ex: England, London</param>
    /// <returns>Longitude and latitude of location</returns>
    public async Task<Geodata?> GetGeodataByQuery(string query)
    {
        var escapedQuery = Uri.EscapeDataString(query);
        var uri = $"{_geoCodingUrl}?q={escapedQuery}&format=json&limit=1";

        var result = await _httpService.GetAsync<Geodata[]>(uri);
        return result.Any() ? result[0] : null;
    }
    
    /// <summary>
    /// Finds coordinates of location, by given parameters
    /// </summary>
    /// <returns>Longitude and latitude of location</returns>
    public async Task<Geodata?> GetGeodataByAddress(Location location)
    {
        var uri = GetUri(location);
        
        var result = await _httpService.GetAsync<Geodata[]>(uri);
        
        return result.Any() ? result[0] : null;
    }

    /// <summary>
    /// Gets 10 addresses near given coordinate
    /// </summary>
    public async Task<IReadOnlyCollection<AddressDto>> GetAddresses(Geodata data)
    {
        var api = new SuggestClientAsync(_token);
        var response = await api.Geolocate(lat: data.Lat, lon: data.Lon, count: 10, radius_meters: 1000);
        
        var addresses = response.suggestions
            .Select(s => s.data)
            .Select(a => a.Adapt<AddressDto>())
            .ToList()
            .AsReadOnly();
        
        return addresses;
    }

    private string GetUri(Location location)
    {
        var encodedStreet = Uri.EscapeDataString(location.Street ?? string.Empty);
        var encodedCity = Uri.EscapeDataString(location.City ?? string.Empty);
        var encodedCountry = Uri.EscapeDataString(location.Country ?? string.Empty);

        return $"{_geoCodingUrl}?country={encodedCountry}&city={encodedCity}&street={encodedStreet}&format=json&limit=1";
    }
}
