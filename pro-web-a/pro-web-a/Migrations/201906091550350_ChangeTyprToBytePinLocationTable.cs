namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTyprToBytePinLocationTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PinLocations", "Type", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PinLocations", "Type", c => c.Boolean(nullable: false));
        }
    }
}
