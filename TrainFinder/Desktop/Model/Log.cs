using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Desktop.Model;
using Newtonsoft.Json;

namespace Desktop
{
    using System;

    public class Log
    {
        public string Name { get; set; }
        public string TrainId { get; set; }
        public string StartTime { get; set; }
        public string MaxSpeed { get; set; }
        public string Speed { get; set; }
        public string Delay { get; set; }
        public string Status { get; set; }
        public string LastLocation { get; set; }
        public string NextStop { get; set; }

        public static async Task<ObservableCollection<Log>> GetActiveTrains()
        {
            var tempData = await WebConnect.GetData("Train/GetActiveTrains");
            var results = JsonConvert.DeserializeObject<IEnumerable<Log>>(tempData);
            var log = new ObservableCollection<Log>();
            foreach (var result in results)
            {
                var temp = new Log
                {
                    TrainId = result.TrainId,
                    Name = result.Name,
                    StartTime = result.StartTime,
                    Speed = result.Speed,
                    Delay = TimeSpan.Parse(result.Delay).ToString(@"hh\:mm\:ss"),
                    Status = result.Status,
                    MaxSpeed = result.MaxSpeed,
                    NextStop = result.NextStop,
                    LastLocation = result.LastLocation,
                    
                };
                log.Add(temp);
            }
            return log;
        }

    }
}
