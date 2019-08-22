using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Configuration;
using System.Web.Http;
using Newtonsoft.Json;
using pro_web_a.Helpers;
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
        private readonly int _defaultSpeed = int.Parse(WebConfigurationManager.AppSettings["DefaultSpeed"]);
        private readonly int _defaultGap = int.Parse(WebConfigurationManager.AppSettings["DefaultGap"]);
        private readonly int _defaultPlatformLength = int.Parse(WebConfigurationManager.AppSettings["DefaultPlatformLength"]);
        private readonly int _defaultPinLocationGap = int.Parse(WebConfigurationManager.AppSettings["DefaultPinLocationGap"]);
        private readonly ProjectDB _dbContext = new ProjectDB();

        private string _writeToFile="";

        private ActiveTrain _activeTrain = null;

        //Get Use to Add Location from device to database 
        
        [HttpGet]
        [Route("Setlocation/")]
        public IHttpActionResult SetLocation(short id, string data)
        {
            ActiveTrainDetails.ActiveTrainDictionary.TryGetValue(id, out _activeTrain);
            //get active train data
            if (_activeTrain == null)//=========================================================== no need
                return Ok();
            //Get relevant LogTable recode
            var logRecord = _dbContext.Log.Single(l => l.TrainId == id);

            //If Train Not At the End Station
            if (logRecord.LogId == -2) return Ok("OK");

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
            var update = _dbContext.Location.FirstOrDefault(l => l.LocationLogId == logRecord.LogId);

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
            update.MaxSpeed = logRecord.MaxSpeed;
            update.LocationData += update.LocationData == null ? string.Empty : ",";
            update.LocationData += locationPacket;
            update.LocationDataTemp += update.LocationDataTemp == null ? string.Empty : " : ";
            update.LocationDataTemp += data;
            _dbContext.SaveChanges();

            var thread1 = new Thread(() =>
            {
                UpdateLog(str1, str2, logRecord.TrainId, logRecord.Direction, speed);
                Send(str2, logRecord.TrainId, logRecord.Direction);
            });
            thread1.Start();
            return Ok("OK");
        }

        //Update LogTable
        private void UpdateLog(SaveData str1, SaveData str2, int trainId, bool direction, double speed)
        {

            if (string.IsNullOrEmpty(str1.Lo))
                return;
            //Last and Current Location difference
            //Last location
            var location = str1.Lo.Split(':');
            var point1 = new GeoCoordinate(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));

            //Current location
            location = str2.Lo.Split(':');
            var point2 = new GeoCoordinate(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));

            var gap = point1.GetDistanceTo(point2);

            //Get relevant LogTable recode
            var log = _dbContext.Log.Single(l => l.TrainId == trainId);

            //Get Log recodes next stop station data
            var station = _dbContext.Stations.Single(s => s.SID == log.NextStop);
            var locationSplit = station.Location.Split(':');
            var nextStop = new GeoCoordinate(Convert.ToDouble(locationSplit[0]), Convert.ToDouble(locationSplit[1]));

            //Get data about next stop station timetable
            var stopAt = _dbContext.StopAts.SingleOrDefault(s =>(s.TID == trainId) && (s.Direction == direction) && (s.SID == station.SID));
            var distance = point2.GetDistanceTo(nextStop);

            Debug.WriteLine("gap between last receive point : " + gap);
            Debug.WriteLine("gap between next stop : " + distance);

            //Train moves
            if (_defaultGap<gap)
            {
                //distance to Next stop station 
                if (_defaultSpeed>speed)
                    speed = _defaultSpeed;
                //Time to arrive next station
                var time = TimeSpan.FromSeconds(Math.Round(distance / (speed * 10 / 36), 2));
                var estimateTime = DateTime.Now.TimeOfDay + time;
                var delay = estimateTime.Subtract(TimeSpan.FromSeconds(stopAt.Atime)).Duration();
                _dbContext.Location.Single(l => l.LocationLogId == log.LogId).Delay = (int) delay.TotalSeconds;
                log.Status = delay.TotalMinutes < 5 ? "Delay less than 5 min" : "Delayed";
                log.Delay = delay;
            }
            //Train stops
            else
            {
                if (_defaultPlatformLength>distance)
                {
                    log.Status = $"Stop At {station.Name}";
                    log.LastStop = log.NextStop;
                    log.NextStop = FindNextStationHelpers.FindNextStation(log.TrainId, log.Direction, log.NextStop);
                    if (log.NextStop == -1)
                    {
                        log.LogId = -2;
                        log.Status = $"At End Station";
                        log.EndTime = DateTime.Now.TimeOfDay.ToString(@"hh\:mm");
                    }
                }
                else 
                {
                    var lastStop = _dbContext.Stations.Single(s => s.SID == log.LastStop);
                    var lastStopLocation = lastStop.Location.Split(':');
                    var distanceToLastStop =
                        point2.GetDistanceTo(new GeoCoordinate(Convert.ToDouble(lastStopLocation[0]),
                            Convert.ToDouble(lastStopLocation[1])));
                    log.Status = _defaultPlatformLength > distanceToLastStop ? $"Stop At {lastStop.Name}" : "Waiting For Signals";
                }
            }
            _dbContext.SaveChanges();
            _writeToFile = $"\nSpeed:{log.Speed},Status:{log.Status},Delay:{log.Delay}";
        }

        //Check and Send Alert
        private void Send(SaveData lastLocationData, short trainId, bool direction)
        {
            //get route rid
            var route = _dbContext.Trains.Single(t => t.TID == trainId).RID;

            var pinLocationList = _dbContext.PinLocation.Where(p => (p.RouteId == route) && (!_activeTrain.PinLocations.Contains(p.PinId)))
                .ToList();
            if (pinLocationList.Count == 0)
                return;

            //Get Current Location 
            var locationString = lastLocationData.Lo;
            var location = locationString.Split(':');
            var currentLocation = new GeoCoordinate(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));


            foreach (var pinLocation in pinLocationList)
            {
                var pinPoint = pinLocation.Location.Split(':');
                var pinGeoCoordinate = new GeoCoordinate(Convert.ToDouble(pinPoint[0]), Convert.ToDouble(pinPoint[1]));
                double l;

                //Send Message
                if (_defaultPinLocationGap > (l = currentLocation.GetDistanceTo(pinGeoCoordinate)))
                {
                    ActiveTrainDetails.ActiveTrainDictionary[trainId].PinLocations.Add(pinLocation.PinId);
                    //var sendPushNotification = NotificationHubHelper.SendPushNotification(
                    //    $"Type {pinLocation.Type} is ahead with in 750 M Description : {pinLocation.Description}");
                    _writeToFile += $",Alert Sent : {pinLocation.PinId}";
                    break;
                }
            }
            FileHandler.WriteToFile(_writeToFile);
            FileHandler.WriteToFile(_activeTrain.file, _writeToFile);
        }
    }
}