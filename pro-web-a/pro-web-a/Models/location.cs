namespace pro_web_a.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("location")]
    public partial class location
    {
        [Key]
        [Column(Order = 0)]
        public byte DID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Datetime { get; set; }

        [Column(TypeName = "xml")]
        public string Locationdata { get; set; }

        public virtual device device { get; set; }
    }
}
