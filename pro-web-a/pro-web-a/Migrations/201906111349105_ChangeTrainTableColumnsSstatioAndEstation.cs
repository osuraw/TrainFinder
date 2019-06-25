namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTrainTableColumnsSstatioAndEstation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trains", "StartStation", c => c.Short(nullable: false));
            AddColumn("dbo.Trains", "EndStation", c => c.Short(nullable: false));
            DropColumn("dbo.Trains", "Sstation");
            DropColumn("dbo.Trains", "Estation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trains", "Estation", c => c.Short(nullable: false));
            AddColumn("dbo.Trains", "Sstation", c => c.Short(nullable: false));
            DropColumn("dbo.Trains", "EndStation");
            DropColumn("dbo.Trains", "StartStation");
        }
    }
}
