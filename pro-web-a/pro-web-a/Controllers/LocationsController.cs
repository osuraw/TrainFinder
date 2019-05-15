using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    struct savedata
    {
        public string time;
        public string Location;
    }
    [RoutePrefix("api/location")]
    public class LocationsController : ApiController
    {
        private projectDB db = new projectDB();
        
        [HttpGet]
        [Route("Setlocation/")]
        [Route("{id}/{data}")]
        public IHttpActionResult Setlocation(byte id,string data)
        {
            char[] split = new[] {','};
            var datal=data.Split(split, StringSplitOptions.RemoveEmptyEntries).ToList();
            savedata str;
            var sb = new StringBuilder(datal[3]);
            sb.AppendFormat(" ");
            sb.Append(datal[4]);
            str.Location = sb.ToString();
            sb.Clear();
            sb.AppendFormat("{0}:{1}:{2}",
                datal[2].Substring(8, 2),
                datal[2].Substring(10, 2),
                datal[2].Substring(12, 2));
            str.time = sb.ToString();
            var rrr = JsonConvert.SerializeObject(str);
            var update = db.locations.FirstOrDefault(r =>r.DID==id&&EntityFunctions.TruncateTime(r.Datetime) == EntityFunctions.TruncateTime(DateTime.Now));
            if (update != null)
            {
                update.LastLocation = str.Location;
                update.TimeSpan = TimeSpan.Parse(str.time);
                update.Locationdata += rrr;
            }

            db.SaveChanges();
            return Ok();
        }

       
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlocation(byte id, location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.DID)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!locationExists(id))
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

        
        [ResponseType(typeof(location))]
        public IHttpActionResult Postlocation(location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.locations.Add(location);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (locationExists(location.DID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = location.DID }, location);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool locationExists(byte id)
        {
            return db.locations.Count(e => e.DID == id) > 0;
        }
    }
}