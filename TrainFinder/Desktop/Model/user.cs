using System;

namespace Desktop.Model
{

    public partial class user
    {
        public byte UID { get; set; }
        public string Name { get; set; }
        public string Uname { get; set; }
        public string Password { get; set; }

        public static bool Login (user user)
        {
            var httpResponseMessage = WebConnect.PostData("User/Login", user);
            if (int.TryParse(httpResponseMessage.Content.ReadAsStringAsync().Result, out int id))
            {
                logininfor.UserId = id;
                logininfor.LogTime = DateTime.Now;
                return true;
            }

            return false;
        }


    }
    public static class logininfor
    {
        public static int UserId { get; set; }
        public static DateTime LogTime { get; set; }
    }
}
