namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLocationLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LocationLogs", "TrainId", c => c.Short(nullable: false));
            DropColumn("dbo.LocationLogs", "DeviceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LocationLogs", "DeviceId", c => c.Byte(nullable: false));
            DropColumn("dbo.LocationLogs", "TrainId");
        }
    }
}
