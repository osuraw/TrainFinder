using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using pro_web_a.DTOs;
using pro_web_a.Models;
using WebGrease.Css.Extensions;

namespace pro_web_a.Controllers
{
    [RoutePrefix("api/Train")]
    public class TrainsController : ApiController
    {
        private readonly ProjectDB _context = new ProjectDB();

        #region Train

        //Get List Of All Train
        public IQueryable<Train> GetTrains()
        {
            return _context.Trains;
        }

        //Get Train by Train Id
        [ResponseType(typeof(Train))]
        public IHttpActionResult GetTrain(short id)
        {
            Train train = _context.Trains.Find(id);
            if (train != null)
                return Ok(train);
            return NotFound();
            
        }

        //Get List of Train By Router Id
        [HttpGet]
        [Route("GetTrainInRoute/{id}")]
        [ResponseType(typeof(Train))]
        public IHttpActionResult GetTrainInRoute(short id = 0)
        {
            var trains = _context.Trains.Where(t => t.RID.Equals(id)).ToList();
            if (trains.Count==0)
                return NotFound();
            return Ok(trains);
        }

        //PUT Update Train
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateTrain(short id, Train train)
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

        //POST Add Train
        [ResponseType(typeof(Train))]
        public IHttpActionResult AddTrain(Train train)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Trains.Add(train);
            _context.SaveChanges();
            return CreatedAtRoute("DefaultApi", new {id = train.TID}, train);
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

        //Get Activate Given Train
        [HttpPut]
        [Route("AddTranToWatch")]
        public IHttpActionResult AddTranToWatch(LogDto data)
        {
            var logRecord = _context.Log.SingleOrDefault(l => l.TrainId == data.TrainId);
            var nextStation = Helpers.FindNextStation(data.TrainId, data.Direction, -1);
            if (nextStation == -5)
                return BadRequest();
            int locationLogId = -2;

            //Add new record to LocationLog If only status is 1(active Train)  
            if (data.Status != "Active")
                return Conflict();
            {
                var location = _context.Location.Create();
                location.TrainId = data.TrainId;
                location.DateTime = DateTime.Now.ToShortDateString();
                _context.SaveChanges();
                locationLogId = location.LocationLogId;
            }

            //check whether there is existing record on log table if not add new one
            if (logRecord != null)
            {
                logRecord.Status = data.Status;
                logRecord.LogId = locationLogId;
                logRecord.Direction = data.Direction;
                logRecord.StartTime = DateTime.Now.TimeOfDay.ToString(@"hh\:mm");
                logRecord.NextStop = nextStation;
            }
            else
            {
                var log = new Log
                {
                    TrainId = data.TrainId,
                    DeviceId = _context.Devices.First(d=>d.TID==data.TrainId).DID,
                    Delay = TimeSpan.Zero,
                    Status = data.Status,
                    LogId = locationLogId,
                    Direction = data.Direction,
                    StartTime = DateTime.Now.TimeOfDay.ToString(@"hh\:mm"),
                    NextStop = nextStation,
                    LastReceive = TimeSpan.Zero.ToString(@"hh\:mm"),
                };
                _context.Log.Add(log);
            }

            _context.SaveChanges();
            return Ok("Activated");
        }

        //Get Currently Activate Train List
        [HttpGet]
        [Route("GetActiveTrains")]
        public IHttpActionResult GetActiveTrains()
        {
            var active = _context.Log.Where(l => l.Status != "Canceled").Include(l => l.Train).ToList();
            return Ok(active.Select(ac=>new {ac.Train.Name, ac.TrainId,ac.DeviceId,ac.StartTime,ac.MaxSpeed,ac.Speed,ac.Delay,ac.Status,ac.LastLocation,NextStop= _context.Stations.First(s => s.SID == ac.NextStop).Name }));
        }

        //Get Inactive Train List
        [HttpGet]
        [Route("InactiveTrains")]
        public IHttpActionResult InactiveTrains()
        {
            var active = _context.Log.Where(l => l.Status != "Canceled").Select(l=>l.TrainId).ToList();
            var trains = _context.Trains.Where(t => !active.Contains(t.TID)).Select(t=>new {t.TID,t.Name,t.StartStation,t.EndStation}).ToList();
            return Ok(trains);
        }

        #endregion
    }
}

//// DELETE: api/Train/5
//[ResponseType(typeof(Train))]
//public IHttpActionResult DeleteTrain(short id)
//{
//Train train = _context.Trains.Find(id);
//    if (train == null)
//{
//    return NotFound();
//}

//_context.Trains.Remove(train);
//_context.SaveChanges();

//return Ok(train);
//}