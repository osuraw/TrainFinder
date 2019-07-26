namespace pro_web_a.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class ReportDto1
    {
        public short TrainId { get; set; }

        public string DateTime { get; set; }

        public double MaxSpeed { get; set; }

        public int Delay { get; set; }
    }
}
