using HCI.Models.Restaurants.Model;
using HCI.Models.Restaurants.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Restaurants.Service
{
    namespace HCI.Models.Restaurants.Service
    {
        public class RestaurantService : IRestaurantService
        {
            private readonly IRestaurantRepository _restaurantRepository;

            public RestaurantService(IRestaurantRepository restaurantRepository)
            {
                _restaurantRepository = restaurantRepository;
            }

            public Restaurant GetById(int id)
            {
                return _restaurantRepository.GetById(id);
            }

            public IEnumerable<Restaurant> GetAll()
            {
                return _restaurantRepository.GetAll();
            }

            public void Add(Restaurant restaurant)
            {
                _restaurantRepository.Add(restaurant);
            }

            public void Update(Restaurant restaurant)
            {
                _restaurantRepository.Update(restaurant);
            }

            public void Delete(int id)
            {
                _restaurantRepository.Delete(id);
            }
        }
    }
}
