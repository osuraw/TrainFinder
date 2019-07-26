namespace pro_web_a.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Train
    {
        
        [Key]
        public short TID { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }

        public short StartStation { get; set; }

        public short EndStation { get; set; }

        [StringLength(150)]
        [Column(TypeName = "varchar")]
        public string Description { get; set; }

        public short RID { get; set; }
        
        public virtual Route route { get; set; }
    }
}
