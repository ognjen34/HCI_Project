using HCI.Models.Locations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Locations.DTO
{
    public class EditLocationArgs
    {
        public Location location { get; }
        public EditLocationArgs(Location location)
        {
            this.location = location;
        }
    }
}
