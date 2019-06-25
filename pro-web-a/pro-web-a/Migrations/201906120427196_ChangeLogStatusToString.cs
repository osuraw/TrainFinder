namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLogStatusToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Logs", "Status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Logs", "Status", c => c.Byte(nullable: false));
        }
    }
}
