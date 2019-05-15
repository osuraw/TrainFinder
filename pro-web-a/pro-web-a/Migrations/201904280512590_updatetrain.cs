namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetrain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.location", "LastLocation", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.location", "TimeSpan", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.location", "Locationdata", c => c.String(maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.location", "Locationdata", c => c.String(storeType: "xml"));
            DropColumn("dbo.location", "TimeSpan");
            DropColumn("dbo.location", "LastLocation");
        }
    }
}
