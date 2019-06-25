namespace pro_web_a.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte UID { get; set; }

        [StringLength(30)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }

        [StringLength(20)]
        public string Uname { get; set; }

        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; }
    }
}
