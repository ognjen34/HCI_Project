using HCI.Models.Trips.Model;
using HCI.Models.Trips.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Service
{
    public class OrderedTripService : IOrderedTripService
    {
        private readonly IOrderedTripRepository _orderedTripRepository;

        public OrderedTripService(IOrderedTripRepository orderedTripRepository)
        {
            _orderedTripRepository = orderedTripRepository;
        }

        public OrderedTrip GetById(int id)
        {
            return _orderedTripRepository.GetById(id);
        }

        public void AddOrderedTrip(OrderedTrip orderedTrip)
        {
            _orderedTripRepository.Add(orderedTrip);
        }

        public void RemoveOrderedTrip(OrderedTrip orderedTrip)
        {
            _orderedTripRepository.Remove(orderedTrip);
        }

        public IEnumerable<OrderedTrip> GetAllOrderedTrips()
        {
            return _orderedTripRepository.GetAll();
        }

        public IEnumerable<OrderedTrip> GetAllByUserId(int id)
        {
            return _orderedTripRepository.GetAllByUserId(id);
        }
    }
}
