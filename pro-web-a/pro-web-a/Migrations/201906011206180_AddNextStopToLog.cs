namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNextStopToLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "NextStop", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "NextStop");
        }
    }
}
