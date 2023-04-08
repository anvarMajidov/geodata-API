using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly IGeodataService _dataService;
    private readonly ILogger<LocationController> _logger;

    public LocationController(IGeodataService dataService, ILogger<LocationController> logger)
    {
        _logger = logger;
        _dataService = dataService;
    }
    
    [HttpGet("geodata/query")]
    public async Task<ActionResult<Geodata>> GetGeoData(string query)
    {
        var result = await _dataService.GetGeodataByQuery(query);

        if (result == null)
        {
            _logger.LogInformation("Geodata was not found for query {@query}", query);
            return NoContent();
        }
        
        _logger.LogInformation("Geodata found for query: {@query} => {Lat}, {Lon}", query, result.Lat, result.Lon);
        return result;
    }

    [HttpGet("geodata/address")]
    public async Task<ActionResult<Geodata>> GetGeoData([FromQuery] Location location)
    {
        var result = await _dataService.GetGeodataByAddress(location);

        if (result == null)
        {
            _logger.LogInformation("No geodata found for location {@Location}", location);
            return NoContent();
        }
        _logger.LogInformation("Geodata found for location: {@location} => {Lat}, {Lon}", location, result.Lat, result.Lon);
        return result;
    }

    [HttpGet("address")]
    public async Task<ActionResult<IReadOnlyCollection<AddressDto>>> GetAddresses([FromQuery] Geodata data)
    {
        var addresses = await _dataService.GetAddresses(data);

        if (addresses.Count == 0)
        {
            _logger.LogInformation("No addresses found for geodata {@Geodata}", data);
            return NoContent();
        }
        
        _logger.LogInformation("For coordinates Lat: {Lat}, {Lon}, these addresses were given {@Addresses}", data.Lat, data.Lon, addresses);
        return Ok(addresses);
    }
}
