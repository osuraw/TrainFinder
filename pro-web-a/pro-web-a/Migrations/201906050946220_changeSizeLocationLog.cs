namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeSizeLocationLog : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LocationLogs", "LocationData", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LocationLogs", "LocationData", c => c.String(maxLength: 8000, unicode: false));
        }
    }
}
