using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    public class UserController : ApiController
    {
        private ProjectDB _context;

        public UserController()
        {
            _context = new ProjectDB();
        }

        [HttpPost]
        public IHttpActionResult Login(User user)
        {
            var result = _context.Users.SingleOrDefault(c => (c.Uname.Equals(user.Uname)) && (c.Password.Equals(user.Password)));
            if (result != null)
                return Ok(new{result.Name,result.UID});
            return NotFound();
        }


        /// <summary>
        /// Create or Update User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/User/Create")]
        public HttpResponseMessage Create(User user)
        {
            if (!ModelState.IsValid)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            if (user.UID == 0)
            {
                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.Created);
                }
                catch (DbUpdateException)
                {
                    return new HttpResponseMessage(HttpStatusCode.Conflict);
                }
            }
            else
            {
                var _user = _context.Users.Single(u => u.UID == user.UID);
                _user.Uname = user.Uname;
                _user.Password = user.Password;
                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
        }
    }
}