using HCI.Models.Accommodations.Model;
using HCI.Models.Accommodations.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Accommodations.Service
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public AccommodationService(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }

        public Accommodation GetById(int id)
        {
            return _accommodationRepository.GetById(id);
        }

        public void AddAccommodation(Accommodation accommodation)
        {
            _accommodationRepository.Add(accommodation);
        }

        public void RemoveAccommodation(Accommodation accommodation)
        {
            _accommodationRepository.Remove(accommodation);
        }

        public IEnumerable<Accommodation> GetAllAccommodations()
        {
            return _accommodationRepository.GetAll();
        }

        public void Update(Accommodation accommodation)
        {
            _accommodationRepository.Update(accommodation);
        }
    }
}
