using HCI.Models.Accommodations.Model;
using HCI.Models.Locations.Model;
using HCI.Models.Pictures.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Attractions.Model
{
    public class Attraction
    {
        public int Id { get; set; }
        public float Price { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public Picture Picture { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string ClassName { get; set; }

        public Attraction()
        {
            IsDeleted = false;
        }
    }
}
