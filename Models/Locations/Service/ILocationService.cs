using HCI.Models.Locations.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCI.Models.Locations.Service
{
    public interface ILocationService
    {
        Task<Location> GetLocationById(int id);
        Task<List<Location>> GetAllLocations();
        Task AddLocation(Location location);
        Task UpdateLocation(Location location);
        Task DeleteLocation(int id);
    }
}