namespace Desktop.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        public byte UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class logininfor
    {
        //public static Usuer user { get; set; }
        public static DateTime logtime { get; set; }
    }
}
