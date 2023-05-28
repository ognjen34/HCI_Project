using HCI.Models.Restaurants.Model;
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
            return _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _dbContext.Restaurants.ToList();
        }

        public void Add(Restaurant restaurant)
        {
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();
        }

        public void Update(Restaurant restaurant)
        {
            _dbContext.Restaurants.Update(restaurant);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var restaurant = GetById(id);
            if (restaurant != null)
            {
                _dbContext.Restaurants.Remove(restaurant);
                _dbContext.SaveChanges();
            }
        }
    }
}
