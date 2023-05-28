using HCI.Models.Locations.Model;

namespace HCI.Models.Restaurants.Model
{
    public enum CuisineType
    {
        Italian,
        Mexican,
        Chinese,
        Japanese,
        Indian,
        French,
        Mediterranean,
        American
    }

    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public CuisineType CuisineType { get; set; }
    }
}