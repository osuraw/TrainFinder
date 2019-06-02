namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdddirectionToLogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Direction", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "Direction");
        }
    }
}
