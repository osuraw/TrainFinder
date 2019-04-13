namespace Desktop.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        public byte UID { get; set; }
        public string Name { get; set; }
        public string Uname { get; set; }
        public string Password { get; set; }
    }
    public static class logininfor
    {
        public static int UserId { get; set; }
        public static DateTime LogTime { get; set; }
    }
}
