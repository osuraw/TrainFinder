namespace pro_web_a.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class StopAt
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey(nameof(train))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short TID { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey(nameof(station))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short SID { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Direction { get; set; }

        public float Atime { get; set; }

        public float Dtime { get; set; }

        public  Station station { get; set; }

        public  Train train { get; set; }
    }
}
