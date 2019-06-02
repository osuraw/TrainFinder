using System.Collections.Generic;

namespace pro_web_a.DTOs
{
    public class TrainDetailsDto
    {
        public string Status { get; set; }
        public string EstimateTimeToArrive { get; set; }
        public string EstimateTimeToDestination { get; set; }
        public List<StopStationDto> StopStationDto { get; set; }
    }
}