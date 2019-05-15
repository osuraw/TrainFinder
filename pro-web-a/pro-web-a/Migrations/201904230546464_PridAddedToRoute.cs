namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PridAddedToRoute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.route", "prid", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.route", "prid");
        }
    }
}
