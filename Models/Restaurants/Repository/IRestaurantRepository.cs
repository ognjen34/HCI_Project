using HCI.Models.Restaurants.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Restaurants.Repository
{
    public interface IRestaurantRepository
    {
        Restaurant GetById(int id);
        IEnumerable<Restaurant> GetAll();
        void Add(Restaurant restaurant);
        void Update(Restaurant restaurant);
        void Delete(int id);
    }
}
