using HCI.Models.Trips.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Trips.DTO
{
    public class OrderTripArgs
    {
        public Trip trip { get; }
        public OrderTripArgs(Trip trip)
        {
            this.trip = trip;
        }
    }
}
