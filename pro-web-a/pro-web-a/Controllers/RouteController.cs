using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Description;
using Newtonsoft.Json;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    public class RouteController : ApiController
    {
        private projectDB _context;

        public RouteController()
        {
            _context=new projectDB();
        }

        [HttpPost]
        public HttpResponseMessage CreateRoute(route route)
        {
            if (ModelState.IsValid)
            {

                if (route.RID==0)
                {
                    _context.routes.Add(route);
                }
                else
                {
                    var routetemp = _context.routes.Single(r => r.RID == route.RID);
                    routetemp.Distance = route.Distance;
                    routetemp.Name=route.Name;
                    routetemp.Sstation= route.Sstation;
                    routetemp.Estation= route.Estation;
                }

                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.Created); 
            }
            else 
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        public IEnumerable<route> GetRoute()
        {

            try
            {
                var re = _context.routes.ToList();
                return re;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Route("Api/Route/DeleteRoute/{id}")]
        [HttpDelete]
        public HttpStatusCode DeleteRoute(short id=0)
        {
            if (id!=0)
            {
                route route = _context.routes.Find(id);
                if (route == null)
                {
                    return HttpStatusCode.Conflict;
                }

                _context.routes.Remove(route);
                _context.SaveChanges();

                return HttpStatusCode.OK;
            }

            return HttpStatusCode.BadRequest;
        }

    }
}
