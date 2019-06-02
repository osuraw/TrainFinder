namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationLastrecivetimechang : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LocationLogs", "LastReceive", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LocationLogs", "LastReceive", c => c.DateTime(nullable: false));
        }
    }
}
