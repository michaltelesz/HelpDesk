namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReinitializeDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calls",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        Description = c.String(),
                        StatusID = c.Int(nullable: false),
                        RequestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .ForeignKey("dbo.Statuses", t => t.StatusID)
                .Index(t => t.StatusID)
                .Index(t => t.RequestID);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReadableID = c.String(maxLength: 12),
                        Description = c.String(),
                        ReceivedDate = c.DateTime(nullable: false),
                        ResolvedDate = c.DateTime(),
                        ComputerID = c.Int(nullable: false),
                        StatusID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Computers", t => t.ComputerID, cascadeDelete: true)
                .ForeignKey("dbo.Statuses", t => t.StatusID)
                .Index(t => t.ReadableID, unique: true)
                .Index(t => t.ComputerID)
                .Index(t => t.StatusID);
            
            CreateTable(
                "dbo.Computers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SerialNo = c.String(),
                        Temporary = c.Boolean(nullable: false),
                        OwnerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.OwnerID, cascadeDelete: true)
                .Index(t => t.OwnerID);
            
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SerialNo = c.String(),
                        TypeID = c.Int(nullable: false),
                        ComputerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Computers", t => t.ComputerID, cascadeDelete: true)
                .ForeignKey("dbo.ComponentTypes", t => t.TypeID, cascadeDelete: true)
                .Index(t => t.TypeID)
                .Index(t => t.ComputerID);
            
            CreateTable(
                "dbo.ComponentTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ComponentTypeCategories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.ComponentTypeCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Statuses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Calls", "StatusID", "dbo.Statuses");
            DropForeignKey("dbo.Calls", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.Requests", "StatusID", "dbo.Statuses");
            DropForeignKey("dbo.Requests", "ComputerID", "dbo.Computers");
            DropForeignKey("dbo.Computers", "OwnerID", "dbo.Customers");
            DropForeignKey("dbo.Components", "TypeID", "dbo.ComponentTypes");
            DropForeignKey("dbo.ComponentTypes", "CategoryID", "dbo.ComponentTypeCategories");
            DropForeignKey("dbo.Components", "ComputerID", "dbo.Computers");
            DropIndex("dbo.ComponentTypes", new[] { "CategoryID" });
            DropIndex("dbo.Components", new[] { "ComputerID" });
            DropIndex("dbo.Components", new[] { "TypeID" });
            DropIndex("dbo.Computers", new[] { "OwnerID" });
            DropIndex("dbo.Requests", new[] { "StatusID" });
            DropIndex("dbo.Requests", new[] { "ComputerID" });
            DropIndex("dbo.Requests", new[] { "ReadableID" });
            DropIndex("dbo.Calls", new[] { "RequestID" });
            DropIndex("dbo.Calls", new[] { "StatusID" });
            DropTable("dbo.Statuses");
            DropTable("dbo.Customers");
            DropTable("dbo.ComponentTypeCategories");
            DropTable("dbo.ComponentTypes");
            DropTable("dbo.Components");
            DropTable("dbo.Computers");
            DropTable("dbo.Requests");
            DropTable("dbo.Calls");
        }
    }
}
