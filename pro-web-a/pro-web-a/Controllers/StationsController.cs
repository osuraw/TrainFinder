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
        private readonly ProjectDB _context = new ProjectDB();

        // GET: api/Stations
        public IQueryable<Station> Getstations()
        {
            return _context.Stations;
        }

        [Route("api/Stations/GetStation/{id}")]
        [ResponseType(typeof(Station))]
        public IHttpActionResult GetStation(short id)
        {
            var station = _context.Stations.Where(s => s.RID.Equals(id)).ToList();
            if (station.Count == 0)
            {
                return NotFound();
            }

            return Ok(station);
        }

        [HttpGet]
        [Route("api/Stations/GetStationInRoute/{id}")]
        [ResponseType(typeof(Station))]
        public IHttpActionResult GetStationInRoute(short id=0)
        {
            var station = _context.Stations.Where(s => s.RID.Equals(id)).Select(s=>new {s.Name,SID = s.SID}).ToList();
            if (station.Count==0)
            {
                return NotFound();
            }

            return Ok(station);
        }

        // PUT: api/Stations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putstation(short id, Station station)
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
                if (!StationExists(id))
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
        [ResponseType(typeof(Station))]
        public HttpResponseMessage AddStation(Station station)
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            _context.Stations.Add(station);
            _context.SaveChanges();

            var res = new HttpResponseMessage(HttpStatusCode.Created);
            res.Content = new StringContent(station.SID.ToString());
            return res;
        }

        // DELETE: api/Stations/5
        [ResponseType(typeof(Station))]
        public HttpStatusCode Deletestation(short id)
        {
            Station station = _context.Stations.Find(id);
            if (station == null)
            {
                return HttpStatusCode.Conflict;
            }

            _context.Stations.Remove(station);
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

        private bool StationExists(short id)
        {
            return _context.Stations.Count(e => e.SID == id) > 0;
        }
    }
}