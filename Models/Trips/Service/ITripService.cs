using HCI.Models.Trips.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Service
{
    public interface ITripService
    {
        Trip GetById(int id);
        void AddTrip(Trip trip);
        void RemoveTrip(Trip trip);
        void UpdateTrip(Trip trip);
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllDeletedAndActive();
    }
}
