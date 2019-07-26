using System;
using System.Collections.Generic;

namespace pro_web_a.DTOs
{
    public class TrainDataDto
    {
        public int TrainId;
        public string TrainName;
        public string Direction;
        public int StationId;
        public string StartStationName;
        public string StartStationDeparture;
        public float StartStationDeparture1;
        public int EndStationId;
        public string EndStationName;
        public string EndStationArrival;
        public float EndStationArrival1;
        public TimeSpan Duration;
    }

    public class OptionDto
    {
        public OptionDto()
        {
            Options=new List<TrainDataDto>();
        }

        public string TimeTaken { get; set; }

        public List<TrainDataDto> Options { get; set; }
    }

    public class SearchTrainResult
    {
        public SearchTrainResult()
        {
            Result=new List<OptionDto>();
        }

        public string Count { get; set; } = "0";
        public List<OptionDto> Result { get; set; }
    }
}