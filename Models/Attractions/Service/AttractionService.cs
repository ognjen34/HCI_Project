using HCI.Models.Attractions.Model;
using HCI.Models.Attractions.Repository;
using System.Collections.Generic;

namespace HCI.Models.Attractions.Service
{
    public class AttractionService : IAttractionService
    {
        private readonly IAttractionRepository attractionRepository;

        public AttractionService(IAttractionRepository attractionRepository)
        {
            this.attractionRepository = attractionRepository;
        }

        public Attraction GetById(int id)
        {
            return attractionRepository.GetById(id);
        }

        public IEnumerable<Attraction> GetAll()
        {
            return attractionRepository.GetAll();
        }

        public void Add(Attraction attraction)
        {
            attractionRepository.Add(attraction);
        }

        public void Update(Attraction attraction)
        {
            attractionRepository.Update(attraction);
        }

        public void Delete(int id)
        {
            attractionRepository.Delete(id);
        }

        public IEnumerable<Attraction> GetAllFromCity(string City)
        {
            return attractionRepository.GetAllFromCity(City);
        }
    }
}
