using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly IGeodataService _dataService;

    public LocationController(IGeodataService dataService)
    {
        _dataService = dataService;
    }
    
    [HttpGet("geodata/query")]
    public async Task<ActionResult<Geodata>> GetGeoData(string query)
    {
        var result = await _dataService.GetGeodataByQuery(query);

        if (result == null) return NoContent();
        
        return result;
    }

    [HttpGet("geodata/address")]
    public async Task<ActionResult<Geodata>> GetGeoData([FromQuery] Location location)
    {
        var result = await _dataService.GetGeodataByAddress(location);

        if (result == null) return NoContent();
        
        return result;
    }

    [HttpGet("address")]
    public async Task<ActionResult<IReadOnlyCollection<AddressDto>>> GetAddresses([FromQuery] Geodata data)
    {
        var addresses = await _dataService.GetAddresses(data);

        if (addresses.Count == 0) return NoContent();

        return Ok(addresses);
    }
}
