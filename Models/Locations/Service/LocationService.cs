using System.Collections.Generic;
using System.Threading.Tasks;
using HCI.Models.Locations.Model;
using HCI.Models.Locations.Repository;

namespace HCI.Models.Locations.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public Location GetLocationById(int id)
        {
            return _locationRepository.GetLocationById(id);
        }

        public List<Location> GetAllLocations()
        {
            return _locationRepository.GetAllLocations();
        }

        public void AddLocation(Location location)
        {
            _locationRepository.AddLocation(location);
        }

        public void UpdateLocation(Location location)
        {
            _locationRepository.UpdateLocation(location);
        }

        public void DeleteLocation(int id)
        {
            _locationRepository.DeleteLocation(id);
        }
    }
}