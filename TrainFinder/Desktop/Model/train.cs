using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Desktop.Model
{
    public  class Train
    {
        #region Properties

        public short TID { get; set; }
        public string Name { get; set; }
        public string StartStation { get; set; }
        public string EndStation { get; set; }
        public string Description { get; set; }
        public short RID { get; set; }

        public static ObservableCollection<Train> Trains { get; set; }

        #endregion

        #region static methods

        public static async Task<ObservableCollection<Train>> GetTrainByRouteId(int id = 0)
        {
           await GetTrain(id);
           return Trains;
        }

        public static async Task GetTrain(int id)
        {
            var tempData =await WebConnect.GetData("Train/GetTrainInRoute/" + id);
            var results = JsonConvert.DeserializeObject<IEnumerable<Train>>(tempData);
            var trains = new ObservableCollection<Train>(){new Train(){Name = "Select Train"}};
            foreach (var result in results)
            {
                var temp = new Train
                {
                    RID = result.RID,
                    TID = result.TID,
                    Name = result.Name,
                    StartStation = result.StartStation,
                    EndStation = result.EndStation,
                    Description = result.Description
                };
                trains.Add(temp);
            }
            Trains= trains;
        }


        #endregion

    }
}
