using HCI.Models.Attractions.Model;
using HCI.Models.Restaurants.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Restaurants.Service
{
    public interface IRestaurantService
    {
        Restaurant GetById(int id);
        IEnumerable<Restaurant> GetAll();
        IEnumerable<Restaurant> GetAllFromCity(string City);
        void Add(Restaurant restaurant);
        void Update(Restaurant restaurant);
        void Delete(int id);
    }
}
