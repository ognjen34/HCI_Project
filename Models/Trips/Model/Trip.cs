using HCI.Models.Accommodations.Model;
using HCI.Models.Pictures.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.Model
{
    public class Trip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public Accommodation Accommodation { get; set; }
        public Picture Picture { get; set; }

        public Trip()
        {
            IsDeleted = false;
        }
    }

   
}
