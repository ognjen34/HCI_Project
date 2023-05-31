using System.Collections.Generic;
using System.Linq;
using HCI.Models.Locations.Model;
using Microsoft.EntityFrameworkCore;

namespace HCI.Models.Locations.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _dbContext;

        public LocationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Location GetLocationById(int id)
        {
            return _dbContext.Locations.Find(id);
        }

        public List<Location> GetAllLocations()
        {
            return _dbContext.Locations.ToList();
        }

        public void AddLocation(Location location)
        {
            _dbContext.Locations.Add(location);
            _dbContext.SaveChanges();
        }

        public void UpdateLocation(Location location)
        {
            _dbContext.Entry(location).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void DeleteLocation(int id)
        {
            Location location = _dbContext.Locations.Find(id);
            if (location != null)
            {
                _dbContext.Locations.Remove(location);
                _dbContext.SaveChanges();
            }
        }
    }
}
