namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumberToDevice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.device", "Number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.device", "Number");
        }
    }
}
