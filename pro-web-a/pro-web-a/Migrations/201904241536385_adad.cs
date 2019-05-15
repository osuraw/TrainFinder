namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adad : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.location", "DID", "dbo.device");
            AddForeignKey("dbo.location", "DID", "dbo.device", "DID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.location", "DID", "dbo.device");
            AddForeignKey("dbo.location", "DID", "dbo.device", "DID");
        }
    }
}
