namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComponentCall : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComponentCalls",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ComponentID = c.Int(nullable: false),
                        CallID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Calls", t => t.CallID, cascadeDelete: true)
                .ForeignKey("dbo.Components", t => t.ComponentID, cascadeDelete: true)
                .Index(t => t.ComponentID)
                .Index(t => t.CallID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComponentCalls", "ComponentID", "dbo.Components");
            DropForeignKey("dbo.ComponentCalls", "CallID", "dbo.Calls");
            DropIndex("dbo.ComponentCalls", new[] { "CallID" });
            DropIndex("dbo.ComponentCalls", new[] { "ComponentID" });
            DropTable("dbo.ComponentCalls");
        }
    }
}
