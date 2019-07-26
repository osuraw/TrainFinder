using System;
using Newtonsoft.Json.Linq;

namespace Desktop.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Uname { get; set; }
        public string Password { get; set; }

        //public static bool Login (User user)
        //{
        //    var httpResponseMessage = WebConnect.PostData("User/Login", user);
        //    if (int.TryParse(httpResponseMessage.Content.ReadAsStringAsync().Result, out int id))
        //    {
        //        LogInFor.User.UserId = id;
        //        LogInFor.LogTime = DateTime.Now;
        //        return true;
        //    }

        //    return false;
        //}
    }

    public static class LogInFor
    {
        public static User User { get; set; } = new User();
        public static DateTime LogTime { get; set; }

        public static JObject Data
        {
            set
            {
                User.UserId = (byte) value["UID"];
                User.Name =(string) value["Name"];
                LogTime = DateTime.Now;
            }
        }
    }
}
