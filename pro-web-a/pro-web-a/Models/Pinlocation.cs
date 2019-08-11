using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pro_web_a.Models
{
    public class PinLocation
    {
        [Key]
        public short PinId { get; set; }
        public byte Type { get; set; }
        public string Location{ get; set; }
        public string Message { get; set; }
        public string Description { get; set; }

        [ForeignKey("Route")]
        public short RouteId { get; set; }

        public Route Route { get; set; }
    }
}
