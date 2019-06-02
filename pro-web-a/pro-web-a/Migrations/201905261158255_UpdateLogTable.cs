namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "DeviceId", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "DeviceId");
        }
    }
}
