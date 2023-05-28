using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Location> GetById(int id)
        {
            return await _dbContext.Set<Location>().FindAsync(id);
        }

        public async Task<List<Location>> GetAll()
        {
            return await _dbContext.Set<Location>().ToListAsync();
        }

        public async Task Add(Location location)
        {
            _dbContext.Set<Location>().Add(location);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Location location)
        {
            _dbContext.Set<Location>().Update(location);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var location = await _dbContext.Set<Location>().FindAsync(id);
            if (location != null)
            {
                _dbContext.Set<Location>().Remove(location);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}