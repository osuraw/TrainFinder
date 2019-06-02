namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrainNavigationProAddedLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        TrainId = c.Short(nullable: false),
                        DeviceId = c.Byte(nullable: false),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        MaxSpeed = c.Double(nullable: false),
                        Speed = c.Double(nullable: false),
                        Delay = c.Time(precision: 7),
                        Status = c.Byte(nullable: false),
                        LogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TrainId)
                .ForeignKey("dbo.train", t => t.TrainId)
                .Index(t => t.TrainId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logs", "TrainId", "dbo.train");
            DropIndex("dbo.Logs", new[] { "TrainId" });
            DropTable("dbo.Logs");
        }
    }
}
