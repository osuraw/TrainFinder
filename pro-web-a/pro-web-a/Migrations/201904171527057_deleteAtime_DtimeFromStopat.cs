namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteAtime_DtimeFromStopat : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.stopat", "Atime");
            DropColumn("dbo.stopat", "Dtime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.stopat", "Dtime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.stopat", "Atime", c => c.Time(nullable: false, precision: 7));
        }
    }
}
