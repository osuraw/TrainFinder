namespace pro_web_a.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("route")]
    public partial class route
    {
        [Key]
        public short RID { get; set; }

        public short? Sstation { get; set; }

        public short? Estation { get; set; }

        public double? Distance { get; set; }

        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        public int prid { get; set; }

        public  List<station> stations { get; set; }

        public  List<train> trains { get; set; }
    }
}
