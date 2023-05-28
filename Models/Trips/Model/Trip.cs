using HCI.Models.Accommodations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Model
{
    public class Trip
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Accommodation Accommodation { get; set; }
        public string Picture { get; set; }
    }

   
}
