using HCI.Models.Attractions.Model;
using System.Collections.Generic;

namespace HCI.Models.Attractions.Service
{
    public interface IAttractionService
    {
        Attraction GetById(int id);
        IEnumerable<Attraction> GetAll();
        IEnumerable<Attraction> GetAllFromCity(string City);
        void Add(Attraction attraction);
        void Update(Attraction attraction);
        void Delete(int id);
    }
}