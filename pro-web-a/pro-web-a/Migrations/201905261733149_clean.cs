namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clean : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Logs", "Train_TID", "dbo.train");
            DropIndex("dbo.Logs", new[] { "Train_TID" });
            DropTable("dbo.Logs");
        }
        
        public override void Down()
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
                        Train_TID = c.Short(),
                    })
                .PrimaryKey(t => t.TrainId);
            
            CreateIndex("dbo.Logs", "Train_TID");
            AddForeignKey("dbo.Logs", "Train_TID", "dbo.train", "TID");
        }
    }
}
