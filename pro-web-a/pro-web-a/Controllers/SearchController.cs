using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using pro_web_a.Models;
using pro_web_a.DTOs;

namespace pro_web_a.Controllers
{
    [EnableCors("*","*","*")]
    [RoutePrefix("Api/Search")]
    public class SearchController : ApiController
    {
        private readonly ProjectDB _context = new ProjectDB();
        private static List<int> _routeIdList;
        
        #region search trains
        [HttpGet]
        [Route("SearchTrain")]
        [Route("{startStationId:int}/{endStationId:int}")]
        public IHttpActionResult SearchTrain(int startStationId, int endStationId)
        {
            int statStationRouteId = _context.Stations.Single(s => s.SID == startStationId).RID;
            int endStationRouteId = _context.Stations.Single(s => s.SID == endStationId).RID;
            //both stations in same GetRoute
            if (statStationRouteId == endStationRouteId)
            {
                var options = new OptionDto {Options = GetTrains(startStationId, endStationId)};
                var result = new SearchTrainResult();
                result.Result.Add(options);
                result.Count = "1";
                return Ok(result);
            }

            //stations in near by GetRoute
            var startStationPrimaryRId = _context.Routes.Single(r => r.RID == statStationRouteId).prid;
            var endStationPrimaryRId = _context.Routes.Single(r => r.RID == endStationRouteId).prid;
            var flag = (statStationRouteId == endStationPrimaryRId) ? 1 :((endStationRouteId == startStationPrimaryRId) ? 2 :((startStationPrimaryRId == endStationPrimaryRId) ? 3 : 0));
            if (flag>0)
            {
                int findStation =
                    flag == 1 ? endStationRouteId : statStationRouteId ;
                int connectingStation = GetConnectingStation(findStation);
                var list1 = GetTrains(startStationId, connectingStation);
                var list2 = GetTrains(connectingStation, endStationId);
                var result = FilterResult(list1, list2);
                return Ok(result);
            }
            
            {
                _routeIdList = new List<int>();
                _routeIdList=GetRoute(statStationRouteId, endStationRouteId);
                return Ok(_routeIdList);
            }
           
        }

        private SearchTrainResult FilterResult(List<TrainDataDto> list1, List<TrainDataDto> list2)
        {
            SearchTrainResult result = new SearchTrainResult();
            foreach (TrainDataDto dataDto in list1)
            {
                var data =new OptionDto();
                foreach (TrainDataDto trainDataDto in list2)
                {
                    if (trainDataDto.StartStationDeparture >= dataDto.EndStationArrival)
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
                if(data.Options.Count>0)
                    result.Result.Add(data);
            }

            result.Count =  result.Result.Count.ToString();
            return result;
        }

        private int GetConnectingStation(int findStation)
        {
            // ReSharper disable once PossibleInvalidOperationException
            return (int) _context.Routes.Single(r => r.RID == findStation).Sstation;
        }

        private List<int> GetRoute(int srid, int erid)
        {
            Stack<int> Start = GetStack(srid);
            Stack<int> End = GetStack(erid);
            int temp=0;
            
            while (Start.Count>0)
            {
                if (Start.Peek()==End.Peek())
                {
                    temp = Start.Peek();
                    Start.Pop();
                    End.Pop();
                }
                else
                {
                    if (temp!=0)
                    {
                        End.Push(temp);
                        temp = 0;
                    }
                    End.Push(Start.Pop());
                }
            }

            return End.ToList();
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

            var flag = false; //down line

            //use to find direction
            var distance1 = _context.Stations.Single(s => s.SID == startStationId).Distance;
            var distance2 = _context.Stations.Single(s => s.SID == endStationId).Distance;

            flag = !(distance1 > distance2);

            //list of trains stop at start station
            var selectedRecords = _context.StopAts.Include(s=>s.station)
                .Where(s => s.SID == startStationId && s.Direction == flag && s.Dtime >= time).ToList();

            //check for matching records with end station
            foreach (var record in selectedRecords)
            {
                var matchingRecord = _context.StopAts.Include(s=>s.station).Include(s=>s.train).SingleOrDefault(s =>
                    s.SID == endStationId && s.TID == record.TID && s.Direction == flag);
                if(matchingRecord!=null)
                {
                    //if match add to possible selections
                    var data = new TrainDataDto()
                    {
                        TrainId = record.TID,
                        TrainName = record.train.Name,
                        StationId = record.SID,
                        StartStationName = record.station.Name,
                        StartStationDeparture = record.Dtime,
                        EndStationId = matchingRecord.SID,
                        EndStationName = matchingRecord.station.Name,
                        EndStationArrival = matchingRecord.Atime,
                        Duration = 1000
                        //EndStationDeparture = matchingRecord.Dtime
                        //StartStationArrival = record.Atime,
                    };
                    trainDataDto.Add(data);
                }
            }
            return trainDataDto;
        }

        private string SerializeData(List<TrainDataDto> options)
        {
            return JsonConvert.SerializeObject(options);
        }

        #endregion

        #region Station search

        [Route("GetStation")]
        [Route("{sid:int}")]
        private IHttpActionResult GetStation(int sid)
        {

            var station = _context.StopAts.Where(s => s.SID == sid).Include(s=>s.train).Select(s => new {s.train.Name,s.Atime,s.Dtime,s.station.Address,});

            return Ok();
        }
        #endregion

        #region TrainStatus

        [HttpGet]
        [Route("GetTrainDetails")]
        public IHttpActionResult GetTrainDetails(int trainId,int start,int end,bool direction)
        {
            var data1 = _context.Log.Single(l => l.TrainId == trainId);
            var data2 = _context.StopAts.Where(s => s.TID == trainId && s.Direction == direction)
                .OrderBy(s => s.Atime)
                .Select(s=>new StopStationDto{StationId = (byte) s.SID,ArriveTime=s.Atime.ToString(),DepartureTime=s.Dtime.ToString()})
                .ToList();
            var details =new  TrainDetailsDto
            {
                EstimateTimeToArrive = "00000",
                EstimateTimeToDestination = "00000",
                Status = data1.Status.ToString(),
                StopStationDto = data2
            };
            return Ok(details);
        }

        [HttpGet]
        [Route("GetLocation")]
        public IHttpActionResult GetLocation(int trainId)
        {
            var data1 = _context.Log.Single(l => l.TrainId == trainId);
            var details =new  LocationDto
            {
                TrainId = data1.TrainId.ToString("D"),
                Speed = data1.Speed.ToString("F"),
                Location = data1.LastLocation
            };
            return Ok(details);
        }


        #endregion

        #region StationData

        [HttpGet]
        [Route("GetStations")]
        public IHttpActionResult GetStations()
        {
            var data = _context.Stations.ToList().Select(s => new {s.Name, s.SID});
            return Ok(data);
        }

        #endregion
    }
}