using HCI.Models.Accommodations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Accommodations.Service
{
    public interface IAccommodationService
    {
        Accommodation GetById(int id);
        void AddAccommodation(Accommodation accommodation);
        void RemoveAccommodation(Accommodation accommodation);
        IEnumerable<Accommodation> GetAllAccommodations()
}
