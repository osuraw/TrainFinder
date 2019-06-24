using System;

namespace Desktop.Model
{

    public partial class user
    {
        public byte UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public static bool Login (user user)
        {
            var httpResponseMessage = WebConnect.PostData("User/Login", user);
            if (int.TryParse(httpResponseMessage.Content.ReadAsStringAsync().Result, out int id))
            {
                LogInFor.UserId = id;
                LogInFor.LogTime = DateTime.Now;
                return true;
            }

            return false;
        }


    }
    public static class LogInFor
    {
        public static int UserId { get; set; }
        public static DateTime LogTime { get; set; }
    }
}
