using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    public class UserController : ApiController
    {
        private readonly ProjectDB _context;

        public UserController()
        {
            _context = new ProjectDB();
        }

        //User Login Check
        [HttpPost]
        public IHttpActionResult Login(User user)
        {
            var result = _context.Users.SingleOrDefault(c => (c.Uname.Equals(user.Uname)) && (c.Password.Equals(user.Password)));
            if (result != null)
                return Ok(new{result.Name,result.UID});
            return NotFound();
        }

        //Create User
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
                catch (DbUpdateException e)
                {
                    Debug.WriteLine(e.Message);
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