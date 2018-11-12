using System;
using System.Collections.Generic;
using System.Text;

namespace Client_SignalR.Models
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public Point Point { get; set; }

        public bool? IsIgnitionActive { get; set; }
    }
}
