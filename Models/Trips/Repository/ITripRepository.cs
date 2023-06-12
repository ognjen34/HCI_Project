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
        public void Delete(int id);
        IEnumerable<Trip> GetAll();
        public IEnumerable<Trip> GetAllDeletedAndActive();
        void Update(Trip trip);
    }
}
