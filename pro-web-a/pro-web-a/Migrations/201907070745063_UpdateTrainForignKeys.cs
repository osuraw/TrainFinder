namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTrainForignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.device", "TID", "dbo.Trains");
            DropForeignKey("dbo.StopAts", "TID", "dbo.Trains");
            AddForeignKey("dbo.device", "TID", "dbo.Trains", "TID", cascadeDelete: true);
            AddForeignKey("dbo.StopAts", "TID", "dbo.Trains", "TID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StopAts", "TID", "dbo.Trains");
            DropForeignKey("dbo.device", "TID", "dbo.Trains");
            AddForeignKey("dbo.StopAts", "TID", "dbo.Trains", "TID");
            AddForeignKey("dbo.device", "TID", "dbo.Trains", "TID");
        }
    }
}
