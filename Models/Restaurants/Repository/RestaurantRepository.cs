using HCI.Models.Attractions.Model;
using HCI.Models.Restaurants.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Restaurants.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly AppDbContext _dbContext;

        public RestaurantRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Restaurant GetById(int id)
        {
            return _dbContext.Restaurants.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _dbContext.Restaurants.Where(r => !r.IsDeleted).ToList();
        }

        public void Add(Restaurant restaurant)
        {
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();
        }

        public void Update(Restaurant restaurant)
        {
            var existingRestaurant = GetById(restaurant.Id);
            if (existingRestaurant != null)
            {
                _dbContext.Entry(existingRestaurant).CurrentValues.SetValues(restaurant);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var restaurant = GetById(id);
            if (restaurant != null)
            {
                restaurant.IsDeleted = true;
                _dbContext.SaveChanges();
            }
        }
        public IEnumerable<Restaurant> GetAllFromCity(string city)
        {
            return _dbContext.Restaurants.Where(a => a.Location.City == city).ToList();
        }
    }
}
