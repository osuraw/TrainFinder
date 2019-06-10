using System;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Http;
using Newtonsoft.Json;
using pro_web_a.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WebGrease.Css.Extensions;

namespace pro_web_a.Controllers
{
    internal struct SaveData
    {
        public string Lt;
        public string Lo;
    }

    [RoutePrefix("api/Location")]
    public class LocationsController : ApiController
    {
        private readonly ProjectDB _dbContext = new ProjectDB();

        [HttpGet]
        [Route("Setlocation/")]
        [Route("{id}/{data}")]
        //use to add Location from device to database 
        public IHttpActionResult SetLocation(byte id, string data)
        {
            char[] split = new[] {','};
            var datalist = data.Split(split, StringSplitOptions.RemoveEmptyEntries).ToList();
            SaveData str2;
            var sb = new StringBuilder(datalist[3]);
            sb.AppendFormat(" ");
            sb.Append(datalist[4]);
            str2.Lo = sb.ToString();
            sb.Clear();
            sb.AppendFormat("{0}:{1}:{2}",
                datalist[2].Substring(8, 2),
                datalist[2].Substring(10, 2),
                datalist[2].Substring(12, 2));
            str2.Lt = sb.ToString();
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
                    logRecord.LastLocation= str2.Lo;
                    SaveData str1;
                    str1.Lt = logRecord.LastReceive;
                    str1.Lo = logRecord.LastLocation;
                    logRecord.Speed = speed;
                    logRecord.MaxSpeed = logRecord.MaxSpeed < speed ? speed : logRecord.MaxSpeed;
                    logRecord.LastReceive = str2.Lt;
                    logRecord.LastLocation = str2.Lo;
                    update.LocationData += update.LocationData == null ? string.Empty : ",";
                    update.LocationData += locationPacket;
                    _dbContext.SaveChanges();
                    var thread1 = new Thread(()=>
                    {
                        UpdateLog(str1, str2, trainId, direction, speed);
                        Send(trainId, direction);
                    });
                    thread1.Start();
                  
                }
            }
            return Ok("OK");
        }

        [HttpGet]
        [Route("Test")]
        public IHttpActionResult test()
        {
            Send(1,true);
            return Ok();
        }

        private void Send(short trainId, bool direction)
        {
            //var route = _dbContext.Trains.Find(trainId).RID;
            //var pinLocationList = _dbContext.PinLocation.Where(p=>p.RouteId==route).ToList().f;
            const string accountSid = "ACbe1c5ab385c66d90b7f817e8861b0f90";
            const string authToken = "e6457ba2e522b97e98a04245425294c8";

            TwilioClient.Init(accountSid, authToken);
            var data = new[] {"Osura","1"};
            var message = MessageResource.Create(
                body: $"\nmessage:{data[0]}\ntype - {data[1]}",
                from: new PhoneNumber("+18182769149"),
                to: new  PhoneNumber("+94704686732")
            );

            Debug.WriteLine(message.Sid);
        }

        private void UpdateLog(SaveData str1, SaveData str2,int trainId,bool direction,double speed)
        {
            var location = str1.Lo.Split(' ');
            var point1 = new GeoCoordinate(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));
            location = str2.Lo.Split(' ');
            var point2 = new GeoCoordinate(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));
            var gap = point1.GetDistanceTo(point2);
            var log = _dbContext.Log.Single(l=>l.TrainId==trainId);
            var station = _dbContext.Stations.Single(s => s.SID == log.NextStop);
            var locationSplit = station.Location.Split(':');
            var latitude = Convert.ToDouble(locationSplit[0]);
            var longitude = Convert.ToDouble(locationSplit[1]);
            var stopAt = _dbContext.StopAts.SingleOrDefault(s => (s.TID == trainId) && (s.Direction == direction)&&(s.SID==station.SID));
            if (gap > 70)
            {
                
                var stationLocation =new GeoCoordinate(latitude, longitude);
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
                var val =point2.GetDistanceTo(new GeoCoordinate(latitude, longitude));
                log.Status = val < 50 ? (byte) 4 : (byte) 5;
            }
            _dbContext.SaveChanges();
        }
    }
}