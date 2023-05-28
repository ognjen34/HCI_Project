using HCI.Models.Locations.Model;
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
        public int Beds { get; set; }
        public Location Location { get; set; }
        public List<string> Pictures { get; set; }
        public double PricePerDay { get; set; }

        public Accommodation()
        {
            Pictures = new List<string>();
        }
    }
}
