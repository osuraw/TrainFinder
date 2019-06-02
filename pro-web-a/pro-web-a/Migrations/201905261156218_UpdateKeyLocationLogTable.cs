namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateKeyLocationLogTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationLogs",
                c => new
                    {
                        LocationLogId = c.Int(nullable: false, identity: true),
                        DeviceId = c.Byte(nullable: false),
                        LastReceive = c.DateTime(nullable: false),
                        LocationData = c.String(maxLength: 8000, unicode: false),
                        LastLocation = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.LocationLogId);
            
            DropTable("dbo.Locations");
        }
        
        public override void Down()
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
            
            DropTable("dbo.LocationLogs");
        }
    }
}
