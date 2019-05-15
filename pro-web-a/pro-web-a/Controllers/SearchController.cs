using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using pro_web_a.Models;
using pro_web_a.DTOs;

namespace pro_web_a.Controllers
{
    [RoutePrefix("Api/Search")]
    public class SearchController : ApiController
    {
        private projectDB _context = new projectDB();
        private static List<int> routeIdList;

        [HttpGet]
        [Route("SearchTrain")]
        [Route("{ssId:int}/{esId:int}")]

        #region search trains

        public IHttpActionResult SearchTrain(int ssId, int esId)
        {
            var searchresult = "";

            int statStationRid = _context.stations.Single(s => s.SID == ssId).RID;
            int endStationRid = _context.stations.Single(s => s.SID == esId).RID;

            var spid = _context.routes.Single(r => r.RID == statStationRid).prid;
            var epid = _context.routes.Single(r => r.RID == endStationRid).prid;

            if (statStationRid == endStationRid)
            {
                var Options = GetTrains(ssId, esId);
                searchresult = SerializeData(Options);
                return Ok(Options);
            }
            else if (statStationRid == epid || endStationRid == spid||spid==epid)
            {
                routeIdList = new List<int>();
                routeIdList.Add(statStationRid);
                routeIdList.Add(endStationRid);
                return Ok(routeIdList);
            }
            else
            {
                routeIdList = new List<int>();
                routeIdList=route(statStationRid, endStationRid);
                return Ok(routeIdList);
            }
        }

        private List<int> route(int srid, int erid)
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
               rid = _context.routes.Single(r => r.RID == rid).prid;
               stack.Push(rid);
            } while (rid != 0);
            return stack;
        }
        
        private List<TrainDataDto> GetTrains(int ssId, int esId, float time = 0)
        {
            var TrainData = new List<TrainDataDto>();

            var flag = false; //down line

            var sdistance = _context.stations.Single(s => s.SID == ssId).Distance;
            var edistance = _context.stations.Single(s => s.SID == esId).Distance;

            flag = !(sdistance > edistance);

            var startStationTrainId = from i in _context.stopats
                where i.SID == ssId && i.Direction == flag && i.Dtime >= time
                select i;

            foreach (var stopat in startStationTrainId)
            {
                var etid = _context.stopats.SingleOrDefault(s =>
                    s.SID == esId && s.TID == stopat.TID && s.Direction == flag);
                var data = new TrainDataDto()
                {
                    sid = stopat.SID,
                    satime = stopat.Atime,
                    sdtime = stopat.Dtime,
                    eid = etid.SID,
                    eatime = etid.Atime,
                    edtime = etid.Dtime
                };
                TrainData.Add(data);
            }

            return TrainData;
        }

        private string SerializeData(List<TrainDataDto> options)
        {
            return JsonConvert.SerializeObject(options);
        }

        #endregion

        #region Station search

        [Route("GetStation")]
        [Route("{sid}")]
        private async Task<IHttpActionResult> GetStation(int sid)
        {

            var station = _context.stopats.Where(s => s.SID == sid).Include(s=>s.train).Select(s => new {s.train.Name,s.Atime,s.Dtime,s.station.Address,});

            return Ok("sssss");
        }
        #endregion
        #region TrainStatus



        #endregion
    }
}