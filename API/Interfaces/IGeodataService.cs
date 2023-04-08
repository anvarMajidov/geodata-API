using API.DTOs;

namespace API.Interfaces;

public interface IGeodataService
{
    Task<Geodata?> GetGeodataByQuery(string query);
    Task<Geodata?> GetGeodataByAddress(Location location);
    Task<IReadOnlyCollection<AddressDto>> GetAddresses(Geodata data);
}
