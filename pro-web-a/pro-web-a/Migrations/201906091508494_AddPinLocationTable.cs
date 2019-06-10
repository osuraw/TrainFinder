namespace pro_web_a.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPinLocationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PinLocations",
                c => new
                    {
                        PinId = c.Short(nullable: false, identity: true),
                        Type = c.Boolean(nullable: false),
                        Location = c.String(),
                        Message = c.String(),
                        Description = c.String(),
                        RouteId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.PinId)
                .ForeignKey("dbo.route", t => t.RouteId, cascadeDelete: true)
                .Index(t => t.RouteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PinLocations", "RouteId", "dbo.route");
            DropIndex("dbo.PinLocations", new[] { "RouteId" });
            DropTable("dbo.PinLocations");
        }
    }
}
