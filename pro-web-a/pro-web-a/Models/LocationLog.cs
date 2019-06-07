namespace pro_web_a.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class LocationLog
    {
        [Key]
        public int LocationLogId { get; set; }

        public byte DeviceId { get; set; }

        //public string LastReceive { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string LocationData { get; set; }

        //[Column(TypeName = "varchar"),MaxLength(50)]
        //public string LastLocation { get; set; }

    }
}
