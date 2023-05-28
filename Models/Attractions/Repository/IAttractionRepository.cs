using HCI.Models.Attractions.Model;
using System.Collections.Generic;

namespace HCI.Models.Attractions.Repository
{
    public interface IAttractionRepository
    {
        Attraction GetById(int id);
        IEnumerable<Attraction> GetAll();
        void Add(Attraction attraction);
        void Update(Attraction attraction);
        void Delete(int id);
    }
}