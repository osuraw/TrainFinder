using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    [RoutePrefix("Api/Route")]
    public class RouteController : ApiController
    {
        private readonly ProjectDB _context;

        public RouteController()
        {
            _context = new ProjectDB();
        }

        
        [HttpPost]
        [Route("CreateRoute")]
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
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpPut]
        [Route("UpdateRoute")]
        public IHttpActionResult UpdateRoute(Route route)
        {
            if (ModelState.IsValid)
            {
                var routeTemp = _context.Routes.Single(r => r.RID == route.RID);
                routeTemp.Distance = route.Distance;
                routeTemp.Name = route.Name;
                routeTemp.Sstation = route.Sstation;
                routeTemp.Estation = route.Estation;
                routeTemp.Description = route.Description;
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetRouteList")]
        public IHttpActionResult GetRoute()
        {
            try
            {
                return Ok(_context.Routes.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError();
            }
        }
        
        [HttpGet]
        [Route("Test")]
        public IHttpActionResult ConnectionTest()
        {
            return Ok();
        }
    }
}


//[Route("Api/Route/DeleteRoute/{id}")]
//[HttpDelete]
//public HttpStatusCode DeleteRoute(short id = 0)
//{
//if (id != 0)
//{
//Route route = _context.Routes.Find(id);
//    if (route == null)
//{
//    return HttpStatusCode.Conflict;
//}

//_context.Routes.Remove(route);
//_context.SaveChanges();

//return HttpStatusCode.OK;
//}

//return HttpStatusCode.BadRequest;
//}