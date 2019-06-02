namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastlocationAddedToLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "LastLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "LastLocation");
        }
    }
}
