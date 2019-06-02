namespace pro_web_a.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("station")]
    public class Station
    {
    
        [Key]
        public short SID { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(40)]
        public string Name { get; set; }

        public double Distance { get; set; }

        public double Llongitude { get; set; }

        public double Llatitude { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string Address { get; set; }

        [StringLength(15)]
        [Column(TypeName = "varchar")]
        public string Telephone { get; set; }
        
        public short RID { get; set; }

        public  route route { get; set; }

        public  List<stopat> stopats { get; set; }
    }
}
