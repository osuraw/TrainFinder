using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    [RoutePrefix("Api/TimeTables")]
    public class TimeTablesController : ApiController
    {
        private readonly projectDB _dbContext = new projectDB();

        [HttpGet]
        [Route("GetTimeTableByTypeId")]
        [Route("{type}&{id}")]
        // GET: api/TimeTables/GetTimeTableByTypeId?type=<1-station/2-train>&id=<1>
        [ResponseType(typeof(stopat))]
        public IHttpActionResult GetTimeTableByTypeId(byte type, int id)
        {
            var data = new List<stopat>();

            if (type != 1 && type != 2)
                return BadRequest();

            var tempList = type == 1
                ? _dbContext.stopats.Where(st => st.TID == id).GroupBy(st => st.SID)
                : _dbContext.stopats.Where(st => st.TID == id).GroupBy(st => st.TID);
            foreach (var group in tempList)
            {
                foreach (stopat stopat in group)
                    data.Add(stopat);
            }

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        // PUT: api/TimeTables/sid=/tid=
        [ResponseType(typeof(void))]
        public IHttpActionResult Putstopat(short sid, short tid, stopat[] stopat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var stopadata in stopat)
            {
                if (sid != stopadata.SID || tid != stopadata.TID)
                {
                    return BadRequest();
                }
                _dbContext.Entry(stopadata).State = EntityState.Modified;
            }

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!stopatExists(sid) || !stopatExists(tid))
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

        // POST: api/TimeTables
        [ResponseType(typeof(stopat))]
        public IHttpActionResult Poststopat(stopat[] stopat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (stopat stopat1 in stopat)
            {
                _dbContext.stopats.Add(stopat1);
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

        // DELETE: api/TimeTables/5
        [ResponseType(typeof(stopat))]
        public IHttpActionResult Deletestopat(short id)
        {
            stopat stopat = _dbContext.stopats.Find(id);
            if (stopat == null)
            {
                return NotFound();
            }

            _dbContext.stopats.Remove(stopat);
            _dbContext.SaveChanges();

            return Ok(stopat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool stopatExists(short id)
        {
            return _dbContext.stopats.Count(e => e.TID == id) > 0;
        }
    }
}