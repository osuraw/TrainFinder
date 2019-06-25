using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pro_web_a.DTOs
{
    public class TimeTable
    {
        
        public short TrainId { get; set; }

        public short StationId { get; set; }

        public string TrainName { get; set; }

        public string StationName { get; set; }

        public bool Direction { get; set; }

        public float ArriveTime { get; set; }

        public float DepartureTime { get; set; }
    }
}