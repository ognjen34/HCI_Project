using HCI.Models.Trips.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Service
{
    public interface IOrderedTripService
    {
        OrderedTrip GetById(int id);
        void AddOrderedTrip(OrderedTrip orderedTrip);
        void RemoveOrderedTrip(OrderedTrip orderedTrip);
        IEnumerable<OrderedTrip> GetAllOrderedTrips();
    }
}
