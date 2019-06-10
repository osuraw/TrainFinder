namespace pro_web_a.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("device")]
    public  class device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte DID { get; set; }

        [ForeignKey("train")]
        public short TID { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string Description { get; set; }

        public Train train { get; set; }
    }
}
