using System.Collections.Generic;

namespace pro_web_a.DTOs
{
    public class TrainDataDto
    {
        public int TrainId;
        public string TrainName;
        public int StationId;
        public string StartStationName;
        public float StartStationDeparture;
        public int EndStationId;
        public string EndStationName;
        public float EndStationArrival;
        public float Duration;
        //public float EndStationDeparture;
        //public float StartStationArrival;

    }

    public class OptionDto
    {
        public OptionDto()
        {
            Options=new List<TrainDataDto>();
        }

        public float TimeTaken { get; set; }

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