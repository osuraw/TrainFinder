namespace pro_web_a.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class LocationLog
    {
        public int LocationLogId { get; set; }

        public byte DeviceId { get; set; }

        public string LocationData { get; set; }

        public string LocationDataTemp { get; set; }

        public int Delay { get; set; }
    }
}
