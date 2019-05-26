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
    public class StationsController : ApiController
    {
        private projectDB _context = new projectDB();

        // GET: api/Stations
        public IQueryable<station> Getstations()
        {
            return _context.stations;
        }

        // GET: api/Stations/5
        [ResponseType(typeof(station))]
        public IHttpActionResult Getstation(short id)
        {
            station station = _context.stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }

        [HttpGet]
        [Route("api/Stations/GetStationInRoute/{id}")]
        [ResponseType(typeof(station))]
        public IHttpActionResult GetStationInRoute(short id=0)
        {
            var station = _context.stations.Where(s => s.RID.Equals(id)).Select(s=>new {s.Name,s.SID}).ToList();
            if (station.Count==0)
            {
                return NotFound();
            }

            return Ok(station);
        }

        // PUT: api/Stations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putstation(short id, station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != station.SID)
            {
                return BadRequest();
            }

            _context.Entry(station).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!stationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.Created);
        }

        // POST: api/Stations
        [ResponseType(typeof(station))]
        public HttpResponseMessage AddStation(station station)
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            _context.stations.Add(station);
            _context.SaveChanges();

            var res = new HttpResponseMessage(HttpStatusCode.Created);
            res.Content = new StringContent(station.SID.ToString());
            return res;
        }

        // DELETE: api/Stations/5
        [ResponseType(typeof(station))]
        public HttpStatusCode Deletestation(short id)
        {
            station station = _context.stations.Find(id);
            if (station == null)
            {
                return HttpStatusCode.Conflict;
            }

            _context.stations.Remove(station);
            _context.SaveChanges();

            return HttpStatusCode.OK;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool stationExists(short id)
        {
            return _context.stations.Count(e => e.SID == id) > 0;
        }
    }
}