using HCI.Models.Attractions.Model;
using HCI.Models.Restaurants.Model;
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
        public List<Restaurant> Restaurants { get; set; }
        public List<Attraction> Attractions { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
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
            TimeSpan duration = CheckOut - CheckIn;
            totalPrice += duration.Days * Trip.Accommodation.PricePerDay;

            return totalPrice;
        }
        
    }
   
}
