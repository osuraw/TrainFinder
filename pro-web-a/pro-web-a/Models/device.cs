namespace pro_web_a.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("device")]
    public  class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte DID { get; set; }

        [ForeignKey("train")]
        public short TID { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string Description { get; set; }

        public int Number { get; set; }

        public Train train { get; set; }
    }
}
