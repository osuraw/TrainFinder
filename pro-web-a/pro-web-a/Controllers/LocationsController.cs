using System;
using System.Device.Location;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Http;
using Newtonsoft.Json;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    //Location Time structure
    internal struct SaveData
    {
        public string Lt;
        public string Lo;
    }

    [RoutePrefix("api/Location")]
    public class LocationsController : ApiController
    {
        private readonly ProjectDB _dbContext = new ProjectDB();

        //Get Use to Add Location from device to database 
        [HttpGet]
        [Route("Setlocation/")]
        public IHttpActionResult SetLocation(short id,string data)
        //public IHttpActionResult SetLocation(short id,int device, string data)
        {
            //Get relevant LogTable recode
            var logRecord = _dbContext.Log.Single(l => l.TrainId == id);

            //If Train Not At the End Station
            if (logRecord.LogId != -2)
            {
                //Format Input data
                char[] split = new[] {','};
                var datalist = data.Split(split, StringSplitOptions.RemoveEmptyEntries).ToList();
                SaveData str2;
                var sb = new StringBuilder(datalist[3]);
                sb.AppendFormat(":");
                sb.Append(datalist[4]);
                str2.Lo = sb.ToString();
                sb.Clear();
                sb.AppendFormat("{0}:{1}:{2}",
                    datalist[2].Substring(8, 2),
                    datalist[2].Substring(10, 2),
                    datalist[2].Substring(12, 2));
                var time = DateTime.Parse(sb.ToString());
                str2.Lt = time.ToLocalTime().ToLongTimeString();
                var locationPacket = JsonConvert.SerializeObject(str2);
                var speed = Convert.ToDouble(datalist[6]);

                //Get relevant LocationTable recode
                var update =
                    _dbContext.Location.FirstOrDefault(l => /*l.TrainId == id &&*/ l.LocationLogId == logRecord.LogId);

                //If recode exist
                if (update != null)
                {
                    //Last Location
                    SaveData str1;
                    str1.Lt = logRecord.LastReceive;
                    str1.Lo = logRecord.LastLocation;

                    //Update With New Data
                    logRecord.LastLocation = str2.Lo;
                    logRecord.Speed = speed;
                    logRecord.MaxSpeed = logRecord.MaxSpeed < speed ? speed : logRecord.MaxSpeed;
                    logRecord.LastReceive = str2.Lt;
                    logRecord.LastLocation = str2.Lo;
                    //update.MaxSpeed = logRecord.MaxSpeed;
                    update.LocationData += update.LocationData == null ? string.Empty : ",";
                    update.LocationData += locationPacket;
                    update.LocationDataTemp += update.LocationDataTemp == null ? string.Empty : " : ";
                    update.LocationDataTemp += data;
                    _dbContext.SaveChanges();

                    var thread1 = new Thread(() =>
                    {
                        UpdateLog(str1, str2, logRecord.TrainId, logRecord.Direction, speed);
                        Send(logRecord.TrainId, logRecord.Direction);
                    });
                    thread1.Start();
                }
            }

            //if (device == 1)
            //    return Redirect("https://trainfinderapi.azurewebsites.net/Api/Search/GetTrainDetails?trainId=1&start=1&end=10&direction=1");
            return Ok("OK");
        }

        //Check and Send Alert
        private void Send(short trainId, bool direction)
        {
            var route = _dbContext.Trains.Single(t => t.TID == trainId).RID;
            var pinLocationList = _dbContext.PinLocation.Where(p => p.RouteId == route).ToList();

            //Get Current Location 
            var locationString = _dbContext.Log.Single(l => l.TrainId == trainId).LastLocation;
            var location = locationString.Split(':');
            var currentLocation = new GeoCoordinate(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));


            foreach (var pinLocation in pinLocationList)
            {
                var pinPoint = pinLocation.Location.Split(':');
                var pinGeoCoordinate = new GeoCoordinate(Convert.ToDouble(pinPoint[0]), Convert.ToDouble(pinPoint[1]));

                //Send Message
                if (currentLocation.GetDistanceTo(pinGeoCoordinate) <= 2500)
                {
                    //const string accountSid = "ACbe1c5ab385c66d90b7f817e8861b0f90";
                    //const string authToken = "e6457ba2e522b97e98a04245425294c8";

                    //TwilioClient.Init(accountSid, authToken);
                    //var message = MessageResource.Create(
                    //    body: $"\nmessage:{pinLocation.Message}\ntype - {pinLocation.Type}",
                    //    from: new PhoneNumber("+18182769149"),
                    //    to: new PhoneNumber(_dbContext.Devices.First(d=>d.TID==trainId).Number.ToString())
                    //);
                    Debug.WriteLine("Send Alert");
                }
            }
        }

        //Update LogTable
        private void UpdateLog(SaveData str1, SaveData str2, int trainId, bool direction, double speed)
        {
            if (str1.Lo == null)
                return;
            //Last and Current Location difference
            //Last location
            var location = str1.Lo.Split(':');
            var point1 = new GeoCoordinate(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));
            location = str2.Lo.Split(':');
            //Current location
            var point2 = new GeoCoordinate(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));
            var gap = point1.GetDistanceTo(point2);

            //Get relevant LogTable recode
            var log = _dbContext.Log.Single(l => l.TrainId == trainId);

            //Get Log recodes next stop station data
            var station = _dbContext.Stations.Single(s => s.SID == log.NextStop);
            var locationSplit = station.Location.Split(':');
            var latitude = Convert.ToDouble(locationSplit[0]);
            var longitude = Convert.ToDouble(locationSplit[1]);

            //Get data about next stop station timetable
            var stopAt = _dbContext.StopAts.SingleOrDefault(s =>
                (s.TID == trainId) && (s.Direction == direction) && (s.SID == station.SID));

            //Writetofile($"gap : {gap}");
            //Train moves
            if (gap > 10)
            {
                //distance to Next stop station 
                var stationLocation = new GeoCoordinate(latitude, longitude);
                var distance = point2.GetDistanceTo(stationLocation);

                if (speed < 15)
                    speed = 15;
                //Time to arrive next station
                var time = TimeSpan.FromSeconds(Math.Round(distance / (speed * 10 / 36), 2));
                var estimateTime = DateTime.Now.TimeOfDay + time;
                var delay = estimateTime.Subtract(TimeSpan.FromSeconds(stopAt.Atime)).Duration();
                _dbContext.Location.Single(l => l.TrainId == log.TrainId).Delay = (int) delay.TotalSeconds;
                log.Status = delay.Minutes < 5 ? "Delay less than 5 min" : "Delayed";
                log.Delay = delay;
            }
            //Train stops
            else
            {
                var val = point2.GetDistanceTo(new GeoCoordinate(latitude, longitude));
                //Writetofile($"vale : {val}");
                if (val < 50)
                {
                    log.Status = $"Stop At {station.Name}";
                    log.NextStop = Helpers.FindNextStation(log.TrainId, log.Direction, log.NextStop);
                    if (log.NextStop == -1)
                    {
                        log.LogId = -2;
                        log.Status = $"At End Station";
                    }
                }
                else
                    log.Status = "Waiting For Signals";
            }

            //Writetofile("speed : " + log.Speed + "\n Status : " + log.Status + "\n Next stop : " + log.NextStop);
            _dbContext.SaveChanges();
        }

        private void WriteToFile(string p0)
        {
            var dd = @"D:\MIT\L3S1\log.txt";
            p0 += "\n";
            File.AppendAllText(dd, p0);
        }
    }
}