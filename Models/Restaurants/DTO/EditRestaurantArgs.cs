using HCI.Models.Restaurants.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Restaurants.DTO
{
    public class EditRestaurantArgs
    {
        public Restaurant restaurant { get; }
        public EditRestaurantArgs(Restaurant restaurant)
        {
            this.restaurant = restaurant;
        }
    }
}
