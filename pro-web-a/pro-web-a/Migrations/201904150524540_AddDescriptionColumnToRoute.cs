namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionColumnToRoute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.route", "Description", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.route", "Description");
        }
    }
}
