using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    public class RouteController : ApiController
    {
        private readonly ProjectDB _context;

        public RouteController()
        {
            _context = new ProjectDB();
        }

        
        [HttpPost]
        public HttpResponseMessage CreateRoute(Route route)
        {
            if (ModelState.IsValid)
            {
                if (route.RID == 0)
                {
                    _context.Routes.Add(route);
                }
                else
                {
                    var routeTemp = _context.Routes.Single(r => r.RID == route.RID);
                    routeTemp.Distance = route.Distance;
                    routeTemp.Name = route.Name;
                    routeTemp.Sstation = route.Sstation;
                    routeTemp.Estation = route.Estation;
                    routeTemp.Description = route.Description;
                }

                _context.SaveChanges();

                var res = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(route.RID.ToString())
                };
                return res;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateRoute(Route route)
        {
            if (ModelState.IsValid)
            {
                var routeTtemp = _context.Routes.Single(r => r.RID == route.RID);
                routeTtemp.Distance = route.Distance;
                routeTtemp.Name = route.Name;
                routeTtemp.Sstation = route.Sstation;
                routeTtemp.Estation = route.Estation;
                routeTtemp.Description = route.Description;
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
                var re = _context.Routes.ToList();
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
                Route route = _context.Routes.Find(id);
                if (route == null)
                {
                    return HttpStatusCode.Conflict;
                }

                _context.Routes.Remove(route);
                _context.SaveChanges();

                return HttpStatusCode.OK;
            }

            return HttpStatusCode.BadRequest;
        }


        [HttpGet]
        [Route("Api/Route/Test")]
        public IHttpActionResult ConnectionTest()
        {
            return Ok();
        }
    }
}