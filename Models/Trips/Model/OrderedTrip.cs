using HCI.Models.Attractions.Model;
using HCI.Models.Restaurants.Model;
using HCI.Models.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Model
{
    public class OrderedTrip
    {
        public int Id { get; set; }
        public Trip Trip { get; set; }
        public HashSet<Restaurant> Restaurants { get; set; }
        public HashSet<Attraction> Attractions { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public double TotalPrice => CalculateTotalPrice();
        public User User { get; set; }

        public OrderedTrip()
        {
            Restaurants = new HashSet<Restaurant>();
            Attractions = new HashSet<Attraction>();
        }

        private double CalculateTotalPrice()
        {
            double totalPrice = 0;

            foreach (Attraction attraction in Attractions)
            {
                totalPrice += attraction.Price;
            }
            TimeSpan duration = CheckOut - CheckIn;
            totalPrice += duration.Days * Trip.Accommodation.PricePerDay;

            return totalPrice;
        }
        
    }
   
}
