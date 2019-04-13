namespace pro_web_a.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("stopat")]
    public partial class stopat
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short TID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short SID { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Direction { get; set; }

        public TimeSpan Atime { get; set; }

        public TimeSpan Dtime { get; set; }

        public virtual station station { get; set; }

        public virtual train train { get; set; }
    }
}
