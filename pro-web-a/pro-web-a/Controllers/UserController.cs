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
        private projectDB _context;
        public UserController()
        {
            _context=new projectDB();
        }

        [HttpPost]
        public int Login(user  user)
        {
            var result = _context.users.SingleOrDefault(c => (c.Uname == user.Uname) && (c.Password == user.Password));
            return result?.UID ?? 0;
        }



        /// <summary>
        /// Create or Update User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/User/Create")]
        public HttpResponseMessage Create(user user)
        {
            if (ModelState.IsValid)
            {

                if (user.UID==0)
                {
                    try
                    {
                        _context.users.Add(user);
                        _context.SaveChanges();
                        return new HttpResponseMessage(HttpStatusCode.Created);
                    }
                    catch (DbUpdateException e)
                    {
                        return new HttpResponseMessage(HttpStatusCode.Conflict);
                    }
                }
                else
                {
                    var _user = _context.users.Single(u => u.UID == user.UID);
                    _user.Uname = user.Uname;
                    _user.Password = user.Password;
                    _context.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.Created);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
