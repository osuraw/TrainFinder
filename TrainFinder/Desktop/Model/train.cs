using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Desktop.Model
{
    public  class Train
    {
        #region Properties

        public short TID { get; set; }
        public string Name { get; set; }
        public short Sstation { get; set; }
        public short Estation { get; set; }
        public string Description { get; set; }
        public short RID { get; set; }

        #endregion

        #region static methods

        public static ObservableCollection<Train> GetTrainByRouteId(int id)
        {
            var tempData = WebConnect.GetData("Train/GetTrainInRoute/" + id);
            var results = JsonConvert.DeserializeObject<IEnumerable<Train>>(tempData);
            var trains = new ObservableCollection<Train>(){new Train(){Name = "Select Train"}};
            foreach (var result in results)
            {
                var temp = new Train
                {
                    RID = result.RID,
                    TID = result.TID,
                    Name = result.Name,
                    Sstation = result.Sstation,
                    Estation = result.Estation,
                    Description = result.Description
                };
                trains.Add(temp);
            }
            return trains;
        }


        #endregion

    }
}
