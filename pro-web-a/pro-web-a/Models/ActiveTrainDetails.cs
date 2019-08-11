using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pro_web_a.Models
{
    public static class ActiveTrainDetails
    {
        private static Dictionary<int,List<int>> _activeTrainDictionary=new Dictionary<int, List<int>>();

        public static Dictionary<int, List<int>> ActiveTrainDictionary
        {
            get => _activeTrainDictionary;
            set => _activeTrainDictionary = value;
        }
    }
}