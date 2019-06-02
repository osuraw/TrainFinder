namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastReciveToLogTableAndRemoveLastReciveAndLastlocationFromLocationLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "LastReceive", c => c.String());
            DropColumn("dbo.LocationLogs", "LastReceive");
            DropColumn("dbo.LocationLogs", "LastLocation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LocationLogs", "LastLocation", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.LocationLogs", "LastReceive", c => c.String());
            DropColumn("dbo.Logs", "LastReceive");
        }
    }
}
