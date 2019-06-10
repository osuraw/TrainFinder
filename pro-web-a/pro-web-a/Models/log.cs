namespace pro_web_a.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Log
    {
        [Key]
        [ForeignKey("Train")]
        public short TrainId { get; set; }

        public bool Direction { get; set; }
        public byte DeviceId { get; set; }

        [Column(TypeName = "varchar")]
        public string StartTime { get; set; }

        [Column(TypeName = "varchar")]
        public string EndTime { get; set; }

        public double MaxSpeed { get; set; }

        public double Speed { get; set; }

        public TimeSpan? Delay { get; set; }

        public byte Status { get; set; }

        
        [Column(TypeName = "varchar")]
        public string LastLocation { get; set; }

        [Column(TypeName = "varchar")]
        public string LastReceive { get; set; }

        public int NextStop { get; set; }

        public int LogId { get; set; }

        public Train Train { get; set; }
    }
}
