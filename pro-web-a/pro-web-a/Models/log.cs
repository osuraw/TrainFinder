namespace pro_web_a.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("log")]
    public partial class log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short TID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Datetime { get; set; }

        public TimeSpan? Stime { get; set; }

        public TimeSpan? Etime { get; set; }

        public double? Maxspeed { get; set; }

        public double? Avgspeed { get; set; }

        public TimeSpan? Delay { get; set; }

        public virtual train train { get; set; }
    }
}
