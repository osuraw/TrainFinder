using System.Web.Razor.Text;

namespace pro_web_a.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("station")]
    public partial class station
    {
    
        [Key]
        public short SID { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public double Distance { get; set; }

        public double Llongitude { get; set; }

        public double Llatitude { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(15)]
        public string Telephone { get; set; }
        
        public short RID { get; set; }

        public  route route { get; set; }

        public  List<stopat> stopats { get; set; }
    }
}
