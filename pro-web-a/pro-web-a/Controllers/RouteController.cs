using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Newtonsoft.Json;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    public class RouteController : ApiController
    {
        private projectDB _context;

        public RouteController()
        {
            _context = new projectDB();
        }

        /// <summary>
        /// Add New Route(post)
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns> 
        [HttpPost]
        public HttpResponseMessage CreateRoute(route route)
        {
            if (ModelState.IsValid)
            {
                if (route.RID == 0)
                {
                    _context.routes.Add(route);
                }
                else
                {
                    var routetemp = _context.routes.Single(r => r.RID == route.RID);
                    routetemp.Distance = route.Distance;
                    routetemp.Name = route.Name;
                    routetemp.Sstation = route.Sstation;
                    routetemp.Estation = route.Estation;
                    routetemp.Description = route.Description;
                }

                _context.SaveChanges();

                var res = new HttpResponseMessage(HttpStatusCode.Created);
                res.Content = new StringContent(route.RID.ToString());
                return res;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateRoute(route route)
        {
            if (ModelState.IsValid)
            {
                var routetemp = _context.routes.Single(r => r.RID == route.RID);
                routetemp.Distance = route.Distance;
                routetemp.Name = route.Name;
                routetemp.Sstation = route.Sstation;
                routetemp.Estation = route.Estation;
                routetemp.Description = route.Description;
                _context.SaveChanges();

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        public IHttpActionResult GetRoute()
        {
            try
            {
                var re = _context.routes.ToList();
                var data = JsonConvert.SerializeObject(re);
                return Ok(re);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError();
            }
        }
        

        [Route("Api/Route/DeleteRoute/{id}")]
        [HttpDelete]
        public HttpStatusCode DeleteRoute(short id = 0)
        {
            if (id != 0)
            {
                route route = _context.routes.Find(id);
                if (route == null)
                {
                    return HttpStatusCode.Conflict;
                }

                _context.routes.Remove(route);
                _context.SaveChanges();
                //_context.Database.ExecuteSqlCommandAsync("UPDATE Person SET additionalData = JSON_MODIFY(additionalData, 'append  $.phoneNumbers', @phoneNumber) WHERE Id = '@personId', personIdParam,phoneNumberParam");

                return HttpStatusCode.OK;
            }

            return HttpStatusCode.BadRequest;
        }
    }
}