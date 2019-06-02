using System;
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
    [RoutePrefix("api/Train")]
    public class TrainsController : ApiController
    {
        private readonly ProjectDB _context = new ProjectDB();

        #region Train

        // GET: api/Train
        public IQueryable<train> GetTrains()
        {
            return _context.Trains;
        }

        // GET: api/Train/5
        [ResponseType(typeof(train))]
        public IHttpActionResult GetTrain(short id)
        {
            train train = _context.Trains.Find(id);
            if (train == null)
            {
                return NotFound();
            }

            return Ok(train);
        }

        [HttpGet]
        [Route("GetTrainInRoute/{id}")]
        [ResponseType(typeof(train))]
        public IHttpActionResult GetTrainInRoute(short id = 0)
        {
            var station = _context.Trains.Where(t => t.RID.Equals(id));
            if (!station.Any())
            {
                return NotFound();
            }

            return Ok(station);
        }

        // PUT: api/Train/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateTrain(short id, train train)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != train.TID)
            {
                return BadRequest();
            }

            _context.Entry(train).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Train
        [ResponseType(typeof(train))]
        public IHttpActionResult AddTrain(train train)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Trains.Add(train);
            _context.SaveChanges();
            return CreatedAtRoute("DefaultApi", new {id = train.TID}, train);
        }

        // DELETE: api/Train/5
        [ResponseType(typeof(train))]
        public IHttpActionResult DeleteTrain(short id)
        {
            train train = _context.Trains.Find(id);
            if (train == null)
            {
                return NotFound();
            }

            _context.Trains.Remove(train);
            _context.SaveChanges();

            return Ok(train);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool TrainExists(short id)
        {
            return _context.Trains.Count(e => e.TID == id) > 0;
        }

        #endregion

        #region TrainControl

        [HttpPut]
        [Route("AddTranToWatch")]
        public IHttpActionResult AddTranToWatch(LogDto data)
        {
            var logRecord = _context.Log.SingleOrDefault(l => l.TrainId == data.TrainId);
            var nextStation = Helpers.FindNextStation(data.TrainId, data.Direction,-1);
            int locationLogId = -2;

            //Add new record to LocationLog If only status is 1(active Train)  
            if (data.Status == 1)
            {
                var location = new LocationLog()
                {
                    DeviceId = data.DeviceId
                };
                _context.Location.Add(location);
                _context.SaveChanges();
                locationLogId = location.LocationLogId;
            }

            //check whether there is existing record on log table if not add new one
            if (logRecord != null)
            {
                logRecord.Status = data.Status;
                logRecord.LogId = locationLogId;
                logRecord.Direction = data.Direction;
                logRecord.StartTime = DateTime.Now.TimeOfDay.ToString("g");
                logRecord.NextStop = nextStation;
            }
            else
            {
                var log = new Log
                {
                    TrainId = data.TrainId,
                    DeviceId = data.DeviceId,
                    Status = data.Status,
                    LogId = locationLogId,
                    Direction = data.Direction,
                    StartTime = DateTime.Now.TimeOfDay.ToString("g"),
                    NextStop = nextStation
            };
                _context.Log.Add(log);
            }
            _context.SaveChanges();
            return Ok();
        }

        

        [HttpGet]
        [Route("GetActiveTrains")]
        public IHttpActionResult GetActiveTrains()
        {
            var active = _context.Log.Where(l => l.Status != 0).Include(l=>l.Train).ToList();
            return Ok(active);
        }
        #endregion
    }
}