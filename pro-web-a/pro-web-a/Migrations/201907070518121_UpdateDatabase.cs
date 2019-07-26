namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LocationLogs", "DateTime", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.LocationLogs", "MaxSpeed", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LocationLogs", "MaxSpeed");
            DropColumn("dbo.LocationLogs", "DateTime");
        }
    }
}
