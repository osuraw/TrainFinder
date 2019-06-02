using System;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Http;
using Newtonsoft.Json;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    internal struct SaveData
    {
        public string LocationTime;
        public string Location;
    }

    [RoutePrefix("api/Location")]
    public class LocationsController : ApiController
    {
        private readonly ProjectDB _dbContext = new ProjectDB();

        [HttpGet]
        [Route("Setlocation/")]
        [Route("{id}/{data}")]
        public IHttpActionResult SetLocation(byte id, string data)
        {
            char[] split = new[] {','};
            var datalist = data.Split(split, StringSplitOptions.RemoveEmptyEntries).ToList();
            SaveData str2;
            var sb = new StringBuilder(datalist[3]);
            sb.AppendFormat(" ");
            sb.Append(datalist[4]);
            str2.Location = sb.ToString();
            sb.Clear();
            sb.AppendFormat("{0}:{1}:{2}",
                datalist[2].Substring(8, 2),
                datalist[2].Substring(10, 2),
                datalist[2].Substring(12, 2));
            str2.LocationTime = sb.ToString();
            var locationPacket = JsonConvert.SerializeObject(str2);
            var logRecord = _dbContext.Log.Single(l => l.DeviceId == id);
            var trainId = logRecord.TrainId;
            var direction = logRecord.Direction;
            var speed = Convert.ToDouble(datalist[6]);
            if (logRecord.LogId != -2)
            {
                var update = _dbContext.Location.FirstOrDefault(l => l.DeviceId == id && l.LocationLogId == logRecord.LogId);
                if(update!=null)
                {
                    logRecord.LastLocation= str2.Location;
                    SaveData str1;
                    str1.LocationTime = logRecord.LastReceive;
                    str1.Location = logRecord.LastLocation;
                    logRecord.Speed = speed;
                    logRecord.MaxSpeed = logRecord.MaxSpeed < speed ? speed : logRecord.MaxSpeed;
                    logRecord.LastReceive = str2.LocationTime;
                    logRecord.LastLocation = str2.Location;
                    update.LocationData += update.LocationData == null ? string.Empty : ",";
                    update.LocationData += locationPacket;
                    _dbContext.SaveChanges();
                    var thread = new Thread(()=>UpdateLog(str1, str2,trainId, direction, speed));
                    thread.Start();
                }
            }
            return Ok("OK");
        }

        private void UpdateLog(SaveData str1, SaveData str2,int trainId,bool direction,double speed)
        {
            var location = str1.Location.Split(' ');
            var point1 = new GeoCoordinate(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));
            location = str2.Location.Split(' ');
            var point2 = new GeoCoordinate(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));
            var gap = point1.GetDistanceTo(point2);
            var log = _dbContext.Log.Single(l=>l.TrainId==trainId);
            var station = _dbContext.Stations.Single(s => s.SID == log.NextStop);
            var stopAt = _dbContext.StopAts.SingleOrDefault(s => (s.TID == trainId) && (s.Direction == direction)&&(s.SID==station.SID));
            if (gap > 70)
            {
                var stationLocation =new GeoCoordinate(station.Llatitude,station.Llongitude);
                var distance = point2.GetDistanceTo(stationLocation);
                if (speed<30)
                    speed = 30;
                
                var time = TimeSpan.FromSeconds(Math.Round(distance / (speed * 10 / 36), 2));
                var estimateTime = DateTime.Now.TimeOfDay+time;
                var delay = estimateTime - TimeSpan.FromSeconds(stopAt.Atime);
                if (delay.Minutes>5)
                {
                    log.Delay = delay;
                    log.Status = 3;
                }
                else
                {
                    log.Status = 2;
                }
            }
            else
            {
                var val =point2.GetDistanceTo(new GeoCoordinate(station.Llatitude, station.Llongitude));
                log.Status = val < 50 ? (byte) 4 : (byte) 5;
            }
            _dbContext.SaveChanges();
        }
    }
}