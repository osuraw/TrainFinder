namespace pro_web_a.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class LocationLog
    {
        [Key]
        public int LocationLogId { get; set; }

        public short TrainId { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string LocationData { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string LocationDataTemp { get; set; }

        [Column(TypeName = "varchar")]
        public string DateTime { get; set; }

        public double MaxSpeed { get; set; }

        public int Delay { get; set; }
    }
}
