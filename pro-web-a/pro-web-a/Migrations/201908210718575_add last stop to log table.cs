namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlaststoptologtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "LastStop", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "LastStop");
        }
    }
}
