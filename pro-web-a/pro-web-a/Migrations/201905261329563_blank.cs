namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blank : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Logs", "LogId_TrainId", "dbo.Logs");
            DropIndex("dbo.Logs", new[] { "LogId_TrainId" });
            AddColumn("dbo.Logs", "LogId", c => c.Int(nullable: false));
            DropColumn("dbo.Logs", "LogId_TrainId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logs", "LogId_TrainId", c => c.Short());
            DropColumn("dbo.Logs", "LogId");
            CreateIndex("dbo.Logs", "LogId_TrainId");
            AddForeignKey("dbo.Logs", "LogId_TrainId", "dbo.Logs", "TrainId");
        }
    }
}
