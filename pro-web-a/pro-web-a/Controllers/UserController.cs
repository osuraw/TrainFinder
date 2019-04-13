using System;
using System.Collections.Generic;
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
        public bool Login(UserDto  _user)
        {
            var result = _context.users.SingleOrDefault(c => (c.Uname == _user.UserName) && (c.Password == _user.Password));
            return result != null;
        }
    }
}
