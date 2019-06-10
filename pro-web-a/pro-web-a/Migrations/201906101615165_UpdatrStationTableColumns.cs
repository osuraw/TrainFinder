namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatrStationTableColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.station", "Location", c => c.String());
            DropColumn("dbo.station", "Llongitude");
            DropColumn("dbo.station", "Llatitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.station", "Llatitude", c => c.Double(nullable: false));
            AddColumn("dbo.station", "Llongitude", c => c.Double(nullable: false));
            DropColumn("dbo.station", "Location");
        }
    }
}
