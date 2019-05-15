namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAtime_DtimeInStopatTableToFloat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.stopat", "Atime", c => c.Single(nullable: false));
            AddColumn("dbo.stopat", "Dtime", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.stopat", "Dtime");
            DropColumn("dbo.stopat", "Atime");
        }
    }
}
