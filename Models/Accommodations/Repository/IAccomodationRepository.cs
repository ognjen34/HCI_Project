using HCI.Models.Accommodations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Accommodations.Repository
{
    public interface IAccommodationRepository
    {
        Accommodation GetById(int id);
        void Add(Accommodation accommodation);
        void Remove(Accommodation accommodation);
        IEnumerable<Accommodation> GetAll();
        void Update(Accommodation accommodation);
    }
}
