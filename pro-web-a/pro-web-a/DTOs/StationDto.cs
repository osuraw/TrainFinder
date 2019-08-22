using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pro_web_a.DTOs
{
    [Serializable]
    public class StationDto
    {
        private string name;
        private string address;
        private string phoneNo;
        public List<TrainDataDto2> TrainData= new List<TrainDataDto2>();

        public StationDto(string name, string address, string phoneNo)
        {
            this.name = name;
            this.address = address;
            this.phoneNo = phoneNo;
        }

    }

    public class TrainDataDto2
    {
        public string TrainName { get; set; }
        public string TrainId { get; set; }
        public string TrainStartStation { get; set; }
        public string TrainStartStationTime { get; set; }
        public string TrainEndStation { get; set; }
        public string TrainEndStationTime { get; set; }
        public string TrainArriveTime { get; set; }
        public string TrainDepartureTime { get; set; }
    }
}