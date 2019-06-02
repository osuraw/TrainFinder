namespace pro_web_a.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("train")]
    public partial class train
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public train()
        {
            devices = new HashSet<device>();
            stopats = new HashSet<stopat>();
        }

        [Key]
        public short TID { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }

        public short Sstation { get; set; }

        public short Estation { get; set; }

        [StringLength(150)]
        [Column(TypeName = "varchar")]
        public string Description { get; set; }

        public short RID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<device> devices { get; set; }
        
        public virtual route route { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stopat> stopats { get; set; }
    }
}
