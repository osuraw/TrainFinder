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
    internal struct SaveData
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
            SaveData str;
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
    }
}