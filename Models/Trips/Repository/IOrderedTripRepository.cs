using HCI.Models.Trips.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Repository
{
    public interface IOrderedTripRepository
    {
        OrderedTrip GetById(int id);
        void Add(OrderedTrip orderedTrip);
        void Remove(OrderedTrip orderedTrip);
        IEnumerable<OrderedTrip> GetAll();
    }
}
