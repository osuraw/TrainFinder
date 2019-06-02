namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogAndLocationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LogId = c.Int(nullable: false, identity: true),
                        DeviceId = c.Byte(nullable: false),
                        LastReceive = c.DateTime(nullable: false),
                        LocationData = c.String(maxLength: 8000, unicode: false),
                        LastLocation = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.LogId);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        TrainId = c.Short(nullable: false),
                        StartTime = c.Time(precision: 7),
                        EndTime = c.Time(precision: 7),
                        MaxSpeed = c.Double(nullable: false),
                        Speed = c.Double(nullable: false),
                        Delay = c.Time(precision: 7),
                        Status = c.Byte(nullable: false),
                        LogId_TrainId = c.Short(),
                    })
                .PrimaryKey(t => t.TrainId)
                .ForeignKey("dbo.Logs", t => t.LogId_TrainId)
                .Index(t => t.LogId_TrainId);
            
        }
        
        public override void Down()
        {
            
        }
    }
}
