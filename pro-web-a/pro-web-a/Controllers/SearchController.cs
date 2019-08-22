using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using pro_web_a.Models;
using pro_web_a.DTOs;

namespace pro_web_a.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("Api/Search")]
    public class SearchController : ApiController
    {
        private readonly ProjectDB _context = new ProjectDB();
        private static List<int> _routeIdList;

        #region search trains

        //Get Search Possible Train Combinations
        [HttpGet]
        [Route("SearchTrain")]
        public IHttpActionResult SearchTrain(int startStationId, int endStationId)
        {
            int statStationRouteId = _context.Stations.Single(s => s.SID == startStationId).RID;
            int endStationRouteId = _context.Stations.Single(s => s.SID == endStationId).RID;
            //both stations in same GetRoute
            if (statStationRouteId == endStationRouteId)
            {
                var data = GetTrains(startStationId, endStationId);
                var timeTaken = TimeSpan.FromSeconds(CalculateApproximateTime(data));
                var options = new OptionDto {TimeTaken = timeTaken.ToString("g"), Options = data};
                var result = new SearchTrainResult();
                result.Result.Add(options);
                result.Count = "1";
                return Ok(result);
            }

            //stations in near by GetRoute
            var startStationPrimaryRId = _context.Routes.Single(r => r.RID == statStationRouteId).prid;
            var endStationPrimaryRId = _context.Routes.Single(r => r.RID == endStationRouteId).prid;
            var flag = (statStationRouteId == endStationPrimaryRId)
                ? 1
                : ((endStationRouteId == startStationPrimaryRId)
                    ? 2
                    : ((startStationPrimaryRId == endStationPrimaryRId) ? 3 : 0));
            if (flag > 0)
            {
                int findStation =
                    flag == 1 ? endStationRouteId : statStationRouteId;
                int connectingStation = GetConnectingStation(findStation);
                var list1 = GetTrains(startStationId, connectingStation);
                var list2 = GetTrains(connectingStation, endStationId);
                var result = FilterResult(list1, list2);
                return Ok(result);
            }

            {
                _routeIdList = new List<int>();
                _routeIdList = GetRoute(statStationRouteId, endStationRouteId);
                return Ok(_routeIdList);
            }
        }

        private static double CalculateApproximateTime(IEnumerable<TrainDataDto> data)
        {
            var timeTaken = TimeSpan.Zero;
            foreach (var trainDataDto in data)
            {
                timeTaken += trainDataDto.Duration;
            }

            return Math.Round(timeTaken.TotalSeconds, 0);
        }

        private static SearchTrainResult FilterResult(IEnumerable<TrainDataDto> list1, List<TrainDataDto> list2)
        {
            SearchTrainResult result = new SearchTrainResult();
            foreach (TrainDataDto dataDto in list1)
            {
                var data = new OptionDto();
                foreach (TrainDataDto trainDataDto in list2)
                {
                    if (trainDataDto.StartStationDeparture1 >= dataDto.EndStationArrival1)
                    {
                        if (dataDto.TrainId == trainDataDto.TrainId)
                        {
                            dataDto.EndStationArrival = trainDataDto.EndStationArrival;
                            //dataDto.EndStationDeparture = trainDataDto.EndStationDeparture;
                            dataDto.EndStationId = trainDataDto.EndStationId;
                            data.Options.Add(dataDto);
                            list2.Remove(trainDataDto);
                            break;
                        }
                        else
                        {
                            data.Options.Add(dataDto);
                            data.Options.Add(trainDataDto);
                            break;
                        }
                    }
                }

                if (data.Options.Count <= 0) continue;
                data.TimeTaken = TimeSpan.FromSeconds(CalculateApproximateTime(data.Options)).ToString();
                result.Result.Add(data);
            }

            result.Count = result.Result.Count.ToString();
            return result;
        }

        private int GetConnectingStation(int findStation)
        {
            // ReSharper disable once PossibleInvalidOperationException
            return (int) _context.Routes.Single(r => r.RID == findStation).Sstation;
        }

        private List<int> GetRoute(int srid, int erid)
        {
            Stack<int> start = GetStack(srid);
            Stack<int> end = GetStack(erid);
            int temp = 0;

            while (start.Count > 0)
            {
                if (start.Peek() == end.Peek())
                {
                    temp = start.Peek();
                    start.Pop();
                    end.Pop();
                }
                else
                {
                    if (temp != 0)
                    {
                        end.Push(temp);
                        temp = 0;
                    }

                    end.Push(start.Pop());
                }
            }

            return end.ToList();
        }

        private Stack<int> GetStack(int rid)
        {
            var stack = new Stack<int>();
            stack.Push(rid);
            do
            {
                rid = _context.Routes.Single(r => r.RID == rid).prid;
                stack.Push(rid);
            } while (rid != 0);

            return stack;
        }

        private List<TrainDataDto> GetTrains(int startStationId, int endStationId, float time = 0)
        {
            var trainDataDto = new List<TrainDataDto>();

            //use to find direction
            var distance1 = _context.Stations.Single(s => s.SID == startStationId).Distance;
            var distance2 = _context.Stations.Single(s => s.SID == endStationId).Distance;

            var flag = !(distance1 > distance2);

            //list of trains stop at start station
            var selectedRecords = _context.StopAts.Include(s => s.station)
                .Where(s => s.SID == startStationId && s.Direction == flag && s.Dtime >= time).ToList();

            //check for matching records with end station
            foreach (var record in selectedRecords)
            {
                var matchingRecord = _context.StopAts.Include(s => s.station).Include(s => s.train).SingleOrDefault(s =>
                    s.SID == endStationId && s.TID == record.TID && s.Direction == flag);
                if (matchingRecord != null)
                {
                    var value = Math.Round(matchingRecord.Atime - record.Dtime, 5);
                    var duration = TimeSpan.FromSeconds(value);
                    if (record.Dtime > matchingRecord.Atime)
                        duration = duration.Negate();

                    //if match add to possible selections
                    var data = new TrainDataDto()
                    {
                        TrainId = record.TID,
                        TrainName = record.train.Name,
                        Direction = (flag ? 1 : 0).ToString(),
                        StationId = record.SID,
                        StartStationName = record.station.Name,
                        StartStationDeparture = TimeSpan.FromSeconds(record.Dtime).ToString(),
                        StartStationDeparture1 = record.Dtime,
                        EndStationId = matchingRecord.SID,
                        EndStationName = matchingRecord.station.Name,
                        EndStationArrival = TimeSpan.FromSeconds(matchingRecord.Atime).ToString(),
                        EndStationArrival1 = matchingRecord.Atime,
                        Duration = duration
                    };
                    trainDataDto.Add(data);
                }
            }

            return trainDataDto;
        }

        //private string SerializeData(List<TrainDataDto> options)
        //{
        //    return JsonConvert.SerializeObject(options);
        //}

        #endregion

        #region Station search

        //Get Station Time Table
        [Route("GetStation")]
        public IHttpActionResult GetStation(int sid)
        {
            var station = _context.Stations.Include(s => s.Stops.Select(t => t.train)).Single(s => s.SID == sid);
            var data = new StationDto(station.Name, station.Address, station.Telephone);
            foreach (var stopAt in station.Stops)
            {
                var temp1 = _context.Stations.Single(s => s.SID == stopAt.train.StartStation).Name;
                var temp2 = _context.Stations.Single(s => s.SID == stopAt.train.EndStation).Name;
                data.TrainData.Add(new TrainDataDto2()
                {
                    TrainId = stopAt.train.TID.ToString(),
                    TrainName = stopAt.train.Name,
                    TrainStartStation = stopAt.Direction ? temp1 : temp2,
                    TrainEndStation = stopAt.Direction ? temp2 : temp1,
                    TrainArriveTime = TimeSpan.FromSeconds(stopAt.Atime).ToString(@"h\:mm"),
                    TrainDepartureTime = TimeSpan.FromSeconds(stopAt.Dtime).ToString(@"h\:mm"),
                });
            }

            return Ok(data);
        }

        #endregion

        #region TrainStatus

        //Get Train Stopping Stations
        [HttpGet]
        [Route("GetTrainStopData")]
        public IHttpActionResult
            GetTrainStopData(int trainId, int start, int end, bool direction) //check need of parameters
        {
            var temp = _context.StopAts.Where(s => s.TID == trainId && s.Direction == direction)
                .OrderBy(s => s.Atime)
                .ToList();
            var data = new List<StopStationDto>();
            foreach (var stopAt in temp)
            {
                data.Add(new StopStationDto
                {
                    StationId = (byte) stopAt.SID,
                    ArriveTime = (TimeSpan.FromSeconds(stopAt.Atime)).ToString(),
                    DepartureTime = (TimeSpan.FromSeconds(stopAt.Dtime)).ToString()
                });
            }

            return Ok(data);
        }

        //Get Trains Live Data
        [HttpGet]
        [Route("GetTrainDetails")]
        public IHttpActionResult GetTrainDetails(int trainId, int start, int end, byte direction)
        {
            var data1 = _context.Log.Include(t => t.Train).Single(l => l.TrainId == trainId);
            var distanceNextStation = _context.Stations.Single(s => s.SID == data1.NextStop).Distance;
            var distanceStatStation = _context.Stations.Single(s => s.SID == start).Distance;
            var distanceEndStation = _context.Stations.Single(s => s.SID == end).Distance;
            data1.Speed = data1.Speed < 10 ? 10 : data1.Speed;
            TimeSpan eta = (TimeSpan) (data1.Delay == null
                ? TimeSpan.Zero
                : data1.Delay +
                  TimeSpan.FromHours(Math.Round((Math.Abs(distanceNextStation - distanceStatStation) / data1.Speed),
                      2)));
            TimeSpan etd = (TimeSpan) (data1.Delay == null
                ? TimeSpan.Zero
                : data1.Delay +
                  TimeSpan.FromHours(Math.Round((Math.Abs(distanceEndStation - distanceStatStation) / data1.Speed),
                      2)));


            var details = new TrainDetailsDto
            {
                ETA = eta.ToString(@"hh\:mm\:ss"),
                ETD = etd.ToString(@"hh\:mm\:ss"),
                Status = data1.Status,
                TrainName = data1.Train.Name,
                Speed = data1.Speed.ToString("F"),
                Location = data1.LastLocation
            };
            return Ok(details);
        }

        //Get Trains Live Data
        [HttpGet]
        [Route("GetTrainDetailsApp")]
        public IHttpActionResult GetDetails(int trainId)
        {
            var trainload = _context.Log.Single(l => l.TrainId == trainId);
            var nextStop = _context.Stations.Single(s => s.SID == trainload.NextStop);
            var lastLocation = trainload.LastLocation.Split(':');
            var trainLocation = new GeoCoordinate(Convert.ToDouble(lastLocation[0]), Convert.ToDouble(lastLocation[1]));
            var stationLocation = nextStop.Location.Split(':');
            var nextStationLocation =
                new GeoCoordinate(Convert.ToDouble(stationLocation[0]), Convert.ToDouble(stationLocation[1]));
            var pinLocation = nextStop.Location.Split(':');
            var nextpinLocation =
                new GeoCoordinate(Convert.ToDouble(lastLocation[0]), Convert.ToDouble(lastLocation[1]));


            var reqSpeed = (trainLocation.GetDistanceTo(nextStationLocation) / (trainload.Delay.Value.Hours * 1000))
                .ToString("F");
            //var distance = ;
            var delay = trainload.Delay.Value.ToString(@"hh\:mm\:ss");

            var data = new
            {
                delay,
                nextStop.Name,
                trainload.Speed,
                reqSpeed,
                //distance
            };
            return Ok(data);
        }

        #endregion

        #region StationData

        //Get Stations List
        [HttpGet]
        [Route("GetStations")]
        public IHttpActionResult GetStations()
        {
            var data = _context.Stations.ToList().Select(s => new {s.Name, s.SID});
            return Ok(data);
        }

        #endregion

        #region GetTrains

        [Route("Trains")]
        public IHttpActionResult GetTrains()
        {
            var data = _context.Trains.ToList().Select(t => new {t.TID, t.Name});
            return Ok(data);
        }

        #endregion

        #region Report

        //Get Reports
        [HttpGet]
        [Route("Reports")]
        public IHttpActionResult Reports(int trainId, string parameter1 = "", string parameter2 = "")
        {
            DateTime dateTime1, dateTime2;
            List<ReportDto1> list;
            if (trainId != 0)
            {
                list = _context.Location.Where(l => l.TrainId == trainId)
                    .Select(l => new ReportDto1
                        {TrainId = l.TrainId, DateTime = l.DateTime, Delay = l.Delay, MaxSpeed = l.MaxSpeed})
                    .ToList();
            }
            else
            {
                list = _context.Location
                    .Select(l => new ReportDto1
                        {TrainId = l.TrainId, DateTime = l.DateTime, Delay = l.Delay, MaxSpeed = l.MaxSpeed})
                    .ToList();
            }

            if (!string.IsNullOrEmpty(parameter1))
            {
                dateTime1 = DateTime.Parse(parameter1);
                foreach (var locationLog in list.ToList())
                {
                    if (DateTime.Parse(locationLog.DateTime).CompareTo(dateTime1) < 0)
                    {
                        list.Remove(locationLog);
                    }
                }
            }

            if (!string.IsNullOrEmpty(parameter2))
            {
                dateTime2 = DateTime.Parse(parameter2);
                foreach (var locationLog in list.ToList())
                {
                    if (DateTime.Parse(locationLog.DateTime).CompareTo(dateTime2) > 0)
                    {
                        list.Remove(locationLog);
                    }
                }
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("c1");
            dataTable.Columns.Add("c2");
            dataTable.Columns.Add("c3");
            dataTable.Columns.Add("c4");
            foreach (var reportDto1 in list)
            {
                dataTable.Rows.Add(reportDto1.TrainId, reportDto1.DateTime, reportDto1.Delay, reportDto1.MaxSpeed);
            }

            return Ok(dataTable);
        }

        #endregion
    }
}