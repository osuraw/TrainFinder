using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    public class TrainsController : ApiController
    {
        private projectDB _context = new projectDB();

        // GET: api/Train
        public IQueryable<train> Gettrains()
        {
            return _context.trains;
        }

        // GET: api/Train/5
        [ResponseType(typeof(train))]
        public IHttpActionResult Gettrain(short id)
        {
            train train = _context.trains.Find(id);
            if (train == null)
            {
                return NotFound();
            }

            return Ok(train);
        }

        [HttpGet]
        [Route("api/Train/GetTrainInRoute/{id}")]
        [ResponseType(typeof(train))]
        public IHttpActionResult GetTrainInRoute(short id = 0)
        {
            var station = _context.trains.Where(t => t.RID.Equals(id));
            if (!station.Any())
            {
                return NotFound();
            }

            return Ok(station);
        }

        // PUT: api/Train/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTrain(short id, train train)
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
                if (!trainExists(id))
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
        public IHttpActionResult Posttrain(train train)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.trains.Add(train);
            _context.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = train.TID }, train);
        }

        // DELETE: api/Train/5
        [ResponseType(typeof(train))]
        public IHttpActionResult Deletetrain(short id)
        {
            train train = _context.trains.Find(id);
            if (train == null)
            {
                return NotFound();
            }

            _context.trains.Remove(train);
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

        private bool trainExists(short id)
        {
            return _context.trains.Count(e => e.TID == id) > 0;
        }
    }
}