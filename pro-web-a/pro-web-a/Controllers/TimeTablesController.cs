using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using pro_web_a.DTOs;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    [RoutePrefix("Api/TimeTables")]
    public class TimeTablesController : ApiController
    {
        private readonly ProjectDB _dbContext = new ProjectDB();

        [HttpGet]
        [Route("GetTimeTableByTypeId")]
        [Route("{type}&{id}")]
        // GET: api/TimeTables/GetTimeTableByTypeId?type=<1-station/2-train>&id=<1>
        [ResponseType(typeof(StopAt))]
        public IHttpActionResult GetTimeTableByTypeId(byte type, int id)
        {
            var data = new List<TimeTable>();

            if (type != 1 && type != 2)
                return BadRequest();

            var tempList = type == 1
                ? _dbContext.StopAts.Where(st => st.SID == id)
                    .Select(s => new TimeTable
                    {
                        TrainId = s.TID, StationId = s.SID, ArriveTime = s.Atime, DepartureTime = s.Dtime,
                        Direction = s.Direction
                    }).GroupBy(st => st.StationId).ToList()
                : _dbContext.StopAts.Where(st => st.TID == id)
                    .Select(s => new TimeTable
                    {
                        TrainId = s.TID, StationId = s.SID, ArriveTime = s.Atime, DepartureTime = s.Dtime,
                        Direction = s.Direction
                    })
                    .GroupBy(st => st.TrainId).ToList();
            foreach (var group in tempList)
            {
                foreach (var timeTable in group)
                    data.Add(timeTable);
            }

            if (data.Count == 0)
                return NotFound();
            return Ok(data);
        }

        // PUT: api/TimeTables/sid=/tid=
        [ResponseType(typeof(void))]
        public IHttpActionResult Putstopat(short sid, short tid, StopAt stopAt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (sid != stopAt.SID || tid != stopAt.TID)
            {
                return BadRequest();
            }
            _dbContext.Entry(stopAt).State = EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StopAtExists(sid) || !StopAtExists(tid))
                {
                    return NotFound();
                }
                else
                {
                    return Conflict();
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(StopAt))]
        public IHttpActionResult Poststopat(StopAt[] stopAt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (StopAt stopat1 in stopAt)
            {
                _dbContext.StopAts.Add(stopat1);
            }

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }

            return CreatedAtRoute("DefaultApi", null, new { });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool StopAtExists(short id)
        {
            return _dbContext.StopAts.Count(e => e.TID == id) > 0;
        }
    }
}