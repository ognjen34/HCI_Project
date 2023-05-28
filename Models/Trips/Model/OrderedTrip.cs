using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Model
{
    public class OrderedTrip
    {
        public Trip Trip { get; set; }
        public List<Restaurant> Restaurants { get; set; }
        public List<Attraction> Attractions { get; set; }
        public Duration Duration { get; set; }
        public double TotalPrice => CalculateTotalPrice();

        public OrderedTrip()
        {
            Restaurants = new List<Restaurant>();
            Attractions = new List<Attraction>();
        }

        private double CalculateTotalPrice()
        {
            double totalPrice = 0;

            foreach (Attraction attraction in Attractions)
            {
                totalPrice += attraction.Price;
            }

            foreach (Restaurant restaurant in Restaurants)
            {
                totalPrice += restaurant.Price;
            }

            return totalPrice;
        }
        
    }
    public class Duration
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
