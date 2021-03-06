﻿using System.Collections.Generic;
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

        //GET Get Time Table By Station Or Train
        [HttpGet]
        [Route("GetTimeTableByTypeId")]
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
                {
                    timeTable.TrainName = _dbContext.Trains.First(t => t.TID == timeTable.TrainId).Name;
                    timeTable.StationName = _dbContext.Stations.First(s => s.SID == timeTable.StationId).Name;
                    data.Add(timeTable);
                }
            }

            if (data.Count == 0)
                return NotFound();
            return Ok(data);
        }

        //PUT Update Time Table
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStopAt(short sid, short tid, StopAt stopAt)
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

        //Post Add Recode to Time Table
        [ResponseType(typeof(StopAt))]
        public IHttpActionResult PostStopAt(StopAt stopAt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _dbContext.StopAts.Add(stopAt);

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