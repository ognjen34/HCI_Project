using HCI.Models.Trips.Model;
using HCI.Models.Trips.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Service
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public Trip GetById(int id)
        {
            return _tripRepository.GetById(id);
        }

        public void AddTrip(Trip trip)
        {
            _tripRepository.Add(trip);
        }
        public void UpdateTrip(Trip trip)
        {
            _tripRepository.Update(trip);
        }
        public void RemoveTrip(Trip trip)
        {
            _tripRepository.Remove(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _tripRepository.GetAll();
        }
    }
}
