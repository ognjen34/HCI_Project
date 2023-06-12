using HCI.Models.Trips.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Repository
{
    public class TripRepository : ITripRepository
    {
        private readonly AppDbContext _dbContext;

        public TripRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Trip GetById(int id)
        {
            return _dbContext.Trips.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
        }

        public IEnumerable<Trip> GetAllDeletedAndActive()
        {
            return _dbContext.Trips
                    .OrderBy(t => t.IsDeleted) 
                    .ToList();
        }

        public void Add(Trip trip)
        {
            _dbContext.Trips.Add(trip);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var trip = _dbContext.Trips.FirstOrDefault(a => a.Id == id);
            if (trip != null)
            {
                trip.IsDeleted = true;
                _dbContext.SaveChanges();
            }
}
        public void Update(Trip trip)
        {
            _dbContext.Trips.Update(trip);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Trip> GetAll()
        {
            return _dbContext.Trips.Where(r => !r.IsDeleted).ToList();
        }
    }
}
