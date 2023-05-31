using HCI.Models.Locations.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCI.Models.Locations.Service
{
    public interface ILocationService
    {
        Location GetLocationById(int id);
        List<Location> GetAllLocations();
        void AddLocation(Location location);
        void UpdateLocation(Location location);
        void DeleteLocation(int id);
    }
}