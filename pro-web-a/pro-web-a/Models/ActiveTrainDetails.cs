using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pro_web_a.Models
{
    public static class ActiveTrainDetails
    {
        private static Dictionary<int,ActiveTrain> _activeTrainDictionary=new Dictionary<int, ActiveTrain>();

        public static Dictionary<int, ActiveTrain> ActiveTrainDictionary
        {
            get => _activeTrainDictionary;
            set => _activeTrainDictionary = value;
        }
    }

    public class ActiveTrain
    {
        public List<int> PinLocations;
        public string file;

        public ActiveTrain(string file)
        {
            this.file = file;
            this.PinLocations = new List<int>();
        }
    }


}