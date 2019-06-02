namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangetoVarchar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.device", "Description", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.train", "Name", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.train", "Description", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.route", "Name", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.route", "Description", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.station", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.station", "Address", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.station", "Telephone", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Logs", "StartTime", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Logs", "EndTime", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Logs", "LastLocation", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Logs", "LastReceive", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.user", "Name", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.user", "Password", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.user", "Password", c => c.String(maxLength: 20, fixedLength: true));
            AlterColumn("dbo.user", "Name", c => c.String(maxLength: 30, fixedLength: true));
            AlterColumn("dbo.Logs", "LastReceive", c => c.String());
            AlterColumn("dbo.Logs", "LastLocation", c => c.String());
            AlterColumn("dbo.Logs", "EndTime", c => c.String());
            AlterColumn("dbo.Logs", "StartTime", c => c.String());
            AlterColumn("dbo.station", "Telephone", c => c.String(maxLength: 15, fixedLength: true));
            AlterColumn("dbo.station", "Address", c => c.String(maxLength: 100, fixedLength: true));
            AlterColumn("dbo.station", "Name", c => c.String(nullable: false, maxLength: 20, fixedLength: true));
            AlterColumn("dbo.route", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.route", "Name", c => c.String());
            AlterColumn("dbo.train", "Description", c => c.String(maxLength: 150, fixedLength: true));
            AlterColumn("dbo.train", "Name", c => c.String(maxLength: 50, fixedLength: true));
            AlterColumn("dbo.device", "Description", c => c.String(maxLength: 100, fixedLength: true));
        }
    }
}
