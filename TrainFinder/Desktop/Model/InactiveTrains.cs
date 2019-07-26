using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desktop.Model;
using Newtonsoft.Json;

namespace Desktop
{
    public class InactiveTrains
    {
        public short TID { get; set; }
        public string Name { get; set; }
        public ActionsEnum ActionsEnum { get; set; }
        public DirectionEnum DirectionEnum { get; set; }

        public static async Task<ObservableCollection<InactiveTrains>> GetInActiveTrains()
        {
            var tempData = await WebConnect.GetData("Train/InactiveTrains");
            var results = JsonConvert.DeserializeObject<IEnumerable<InactiveTrains>>(tempData);
            var log = new ObservableCollection<InactiveTrains>();
            foreach (var result in results)
            {
                var temp = new InactiveTrains
                {
                    TID = result.TID,
                    Name = result.Name,
                    ActionsEnum = ActionsEnum.SelectAction,
                    DirectionEnum = DirectionEnum.SelectDirection
                };
                log.Add(temp);
            }
            return log;
        }
    }
}
