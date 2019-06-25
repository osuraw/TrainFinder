namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationdataTempAndDelayToLocationLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LocationLogs", "LocationDataTemp", c => c.String(unicode: false));
            AddColumn("dbo.LocationLogs", "Delay", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LocationLogs", "Delay");
            DropColumn("dbo.LocationLogs", "LocationDataTemp");
        }
    }
}
