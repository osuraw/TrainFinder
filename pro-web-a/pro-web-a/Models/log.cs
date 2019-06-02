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
        
        public string StartTime { get; set; }
       
        public string EndTime { get; set; }

        public double MaxSpeed { get; set; }

        public double Speed { get; set; }

        public TimeSpan? Delay { get; set; }

        public byte Status { get; set; }

        public string LastLocation { get; set; }

        public string LastReceive { get; set; }

        public int NextStop { get; set; }

        public int LogId { get; set; }

        public train Train { get; set; }
    }
}
