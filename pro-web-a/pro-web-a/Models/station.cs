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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public station()
        {
            stopats = new HashSet<stopat>();
        }

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

        public virtual route route { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stopat> stopats { get; set; }
    }
}
