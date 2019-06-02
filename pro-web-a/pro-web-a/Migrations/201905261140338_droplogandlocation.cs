namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class droplogandlocation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.log", "TID", "dbo.train");
            DropForeignKey("dbo.location", "DID", "dbo.device");
            DropIndex("dbo.log", new[] { "TID" });
            DropIndex("dbo.location", new[] { "DID" });
        }
        
        public override void Down()
        {
           
        }
    }
}
