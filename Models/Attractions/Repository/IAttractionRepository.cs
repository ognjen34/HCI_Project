using HCI.Models.Attractions.Model;
using System.Collections.Generic;

namespace HCI.Models.Attractions.Repository
{
    public interface IAttractionRepository
    {
        Attraction GetById(int id);
        IEnumerable<Attraction> GetAll();
        IEnumerable<Attraction> GetAllFromCity(string city);
        void Add(Attraction attraction);
        void Update(Attraction attraction);
        void Delete(int id);
    }
}