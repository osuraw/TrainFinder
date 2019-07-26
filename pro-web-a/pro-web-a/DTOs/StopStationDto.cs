using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pro_web_a.DTOs
{
    public class StopStationDto
    {
        public byte StationId { get; set; }
        public string Name { get; set; }
        public string ArriveTime { get; set; }
        public string DepartureTime { get; set; }
    }
}