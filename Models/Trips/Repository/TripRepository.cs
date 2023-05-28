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
            return _dbContext.Trips.Find(id);
        }

        public void Add(Trip trip)
        {
            _dbContext.Trips.Add(trip);
            _dbContext.SaveChanges();
        }

        public void Remove(Trip trip)
        {
            _dbContext.Trips.Remove(trip);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Trip> GetAll()
        {
            return _dbContext.Trips.ToList();
        }
    }
}
