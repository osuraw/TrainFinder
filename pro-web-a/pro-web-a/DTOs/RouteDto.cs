using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pro_web_a.DTOs
{
    public class RouteDto
    {
        public short RID { get; set; }

        public short Sstation { get; set; }

        public short Estation { get; set; }

        public double Distance { get; set; }

        public string Name { get; set; }
    }
}