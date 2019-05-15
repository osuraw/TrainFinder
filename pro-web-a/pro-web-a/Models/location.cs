namespace pro_web_a.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("location")]
    public partial class location
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("device")]
        public byte DID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Datetime { get; set; }

        [Column(TypeName = "varchar")]
        public string Locationdata { get; set; }
        [Column(TypeName = "varchar"),MaxLength(50)]
        public string LastLocation { get; set; }
        public TimeSpan TimeSpan { get; set; }

        public device device { get; set; }
    }
}
