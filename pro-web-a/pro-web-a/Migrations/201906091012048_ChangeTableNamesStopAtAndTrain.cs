namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTableNamesStopAtAndTrain : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.train", newName: "Trains");
            RenameTable(name: "dbo.stopat", newName: "StopAts");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.StopAts", newName: "stopat");
            RenameTable(name: "dbo.Trains", newName: "train");
        }
    }
}
