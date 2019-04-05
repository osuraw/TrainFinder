using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    public class RouteController : ApiController
    {
        private ProjectDBContext _context;

        public RouteController()
        {
            _context=new ProjectDBContext();
        }

        [HttpPost]
        public int CreateRoute(route route)
        {
            //Debug.WriteLine("Create Route working ");
            if(!ModelState.IsValid)
            { throw new HttpResponseException(HttpStatusCode.BadRequest);}

            _context.routes.Add(route);
            return _context.SaveChanges();

        }

        public IEnumerable<route> GetRoute()
        {
            return _context.routes.ToList();
        }
    }
}
