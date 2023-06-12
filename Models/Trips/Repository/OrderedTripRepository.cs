using HCI.Models.Trips.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Repository
{
    public class OrderedTripRepository : IOrderedTripRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderedTripRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public OrderedTrip GetById(int id)
        {
            return _dbContext.OrderedTrips.Find(id);
        }

        public void Add(OrderedTrip orderedTrip)
        {
            _dbContext.OrderedTrips.Add(orderedTrip);
            _dbContext.SaveChanges();
        }

        public void Remove(OrderedTrip orderedTrip)
        {
            _dbContext.OrderedTrips.Remove(orderedTrip);
            _dbContext.SaveChanges();
        }

        public IEnumerable<OrderedTrip> GetAll()
        {
            return _dbContext.OrderedTrips.ToList();
        }

        public IEnumerable<OrderedTrip> GetAllByUserId(int id)
        {
            return _dbContext.OrderedTrips.Where(t => t.User.Id == id).ToList();
        }
    }
}
