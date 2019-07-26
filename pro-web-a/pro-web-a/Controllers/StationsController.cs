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
    [RoutePrefix("api/Stations")]
    public class StationsController : ApiController
    {
        private readonly ProjectDB _context = new ProjectDB();

        // Get All Stations
        public IHttpActionResult GetStations()
        {
            var station = _context.Stations.Select(s => new { s.Name, s.SID ,s.RID}).ToList();
            if (station.Count == 0)
                return NotFound();
            return Ok(station);
            //return _context.Stations;
        }

        [HttpGet]
        [Route("GetStationInRoute/{id}")]
        [ResponseType(typeof(Station))]
        public IHttpActionResult GetStationInRoute(short id=0)
        {
            var station = _context.Stations.Where(s => s.RID.Equals(id)).ToList();
            if (station.Count==0)
                return NotFound();
            return Ok(station);
        }

        //Put Update Station
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStation(short id, Station station)
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

        //Post Add Station
        [ResponseType(typeof(Station))]
        public HttpResponseMessage AddStation(Station station)
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            _context.Stations.Add(station);
            _context.SaveChanges();

            var res = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(station.SID.ToString())
            };
            return res;
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

////Get Station by Route Id
//[Route("GetStation/{id}")]
//[ResponseType(typeof(Station))]
//public IHttpActionResult GetStation(short id)
//{
//    var station = _context.Stations.Where(s => s.RID.Equals(id)).ToList();
//    if (station.Count == 0)
//        return NotFound();
//    return Ok(station);
//}

////DELETE Station-Not Use
//[ResponseType(typeof(Station))]
//public HttpStatusCode DeleteStation(short id)
//{
//Station station = _context.Stations.Find(id);
//    if (station == null)
//{
//    return HttpStatusCode.Conflict;
//}

//_context.Stations.Remove(station);
//_context.SaveChanges();

//return HttpStatusCode.OK;
//}

//Get Stations By Router Id
//[HttpGet]
//[Route("GetStationInRoute/{id}")]
//[ResponseType(typeof(Station))]
//public IHttpActionResult GetStationInRoute(short id=0)
//{
//    var station = _context.Stations.Where(s => s.RID.Equals(id)).Select(s=>new {s.Name,s.SID}).ToList();
//    if (station.Count==0)
//        return NotFound();
//    return Ok(station);
//}