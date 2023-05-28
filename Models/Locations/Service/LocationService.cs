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

        public async Task<Location> GetLocationById(int id)
        {
            return await _locationRepository.GetById(id);
        }

        public async Task<List<Location>> GetAllLocations()
        {
            return await _locationRepository.GetAll();
        }

        public async Task AddLocation(Location location)
        {
            await _locationRepository.Add(location);
        }

        public async Task UpdateLocation(Location location)
        {
            await _locationRepository.Update(location);
        }

        public async Task DeleteLocation(int id)
        {
            await _locationRepository.Delete(id);
        }
    }
}