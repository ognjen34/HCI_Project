using HCI.Models.Trips.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Repository
{
    public interface ITripRepository
    {
        Trip GetById(int id);
        void Add(Trip trip);
        void Remove(Trip trip);
        IEnumerable<Trip> GetAll();
    }
}
