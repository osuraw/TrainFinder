namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogTableUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Logs", "StartTime", c => c.String());
            AlterColumn("dbo.Logs", "EndTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Logs", "EndTime", c => c.Time(precision: 7));
            AlterColumn("dbo.Logs", "StartTime", c => c.Time(precision: 7));
        }
    }
}
