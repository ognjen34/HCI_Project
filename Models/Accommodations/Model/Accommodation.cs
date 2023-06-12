using HCI.Models.Locations.Model;
using HCI.Models.Pictures.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HCI.Models.Accommodations.Model
{
    public class Accommodation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Beds { get; set; }
        public Location Location { get; set; }
        public List<Picture> Pictures { get; set; }
        public bool IsDeleted { get; set; }
        public double PricePerDay { get; set; }

        public Accommodation()
        {
            IsDeleted = false;
            Pictures = new List<Picture>();
        }
    }

}
