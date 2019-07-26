using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    [RoutePrefix("Api/PinLocation")]
    public class PinLocationController : ApiController
    {
        private ProjectDB db = new ProjectDB();
        
        //Get Pined Location in Route
        [HttpGet]
        [Route("GetPinLocation")]
        public IHttpActionResult GetPinLocation(short rid)
        {
            var pinLocation = db.PinLocation.Where(p=>p.RouteId==rid).ToList();
            if (pinLocation == null)
            {
                return NotFound();
            }

            return Ok(pinLocation);
        }

        //PUT Update Pined Location
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult PutPinLocation(short lid, PinLocation pinLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (lid != pinLocation.PinId)
            {
                return BadRequest();
            }

            db.Entry(pinLocation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PinLocationExists(lid))
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

        //POST Add New Pin Location
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult PostPinLocation(PinLocation pinLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PinLocation.Add(pinLocation);
            db.SaveChanges();
            return Ok(pinLocation.PinId);

            //return CreatedAtRoute("DefaultApi", new { id = pinLocation.PinId }, pinLocation);
        }

        //DELETE Pined Location
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult DeletePinLocation(short lid)
        {
            PinLocation pinLocation = db.PinLocation.Find(lid);
            if (pinLocation == null)
            {
                return NotFound();
            }

            db.PinLocation.Remove(pinLocation);
            db.SaveChanges();

            return Ok(pinLocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PinLocationExists(short id)
        {
            return db.PinLocation.Count(e => e.PinId == id) > 0;
        }
    }
}