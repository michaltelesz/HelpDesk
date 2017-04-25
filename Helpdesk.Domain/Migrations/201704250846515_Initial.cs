namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calls",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Status_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Status", t => t.Status_ID)
                .Index(t => t.Status_ID);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SerialNo = c.String(),
                        Type_ID = c.Int(),
                        Computer_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ComponentTypes", t => t.Type_ID)
                .ForeignKey("dbo.Computers", t => t.Computer_ID)
                .Index(t => t.Type_ID)
                .Index(t => t.Computer_ID);
            
            CreateTable(
                "dbo.ComponentTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Category_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ComponentTypeCategories", t => t.Category_ID)
                .Index(t => t.Category_ID);
            
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
                "dbo.Computers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SerialNo = c.String(),
                        Temporary = c.Boolean(nullable: false),
                        Owner_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.Owner_ID)
                .Index(t => t.Owner_ID);
            
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
                "dbo.Requests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ReceivedDate = c.DateTime(nullable: false),
                        ResolvedDate = c.DateTime(nullable: false),
                        Computer_ID = c.Int(),
                        Status_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Computers", t => t.Computer_ID)
                .ForeignKey("dbo.Status", t => t.Status_ID)
                .Index(t => t.Computer_ID)
                .Index(t => t.Status_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "Status_ID", "dbo.Status");
            DropForeignKey("dbo.Requests", "Computer_ID", "dbo.Computers");
            DropForeignKey("dbo.Computers", "Owner_ID", "dbo.Customers");
            DropForeignKey("dbo.Components", "Computer_ID", "dbo.Computers");
            DropForeignKey("dbo.Components", "Type_ID", "dbo.ComponentTypes");
            DropForeignKey("dbo.ComponentTypes", "Category_ID", "dbo.ComponentTypeCategories");
            DropForeignKey("dbo.Calls", "Status_ID", "dbo.Status");
            DropIndex("dbo.Requests", new[] { "Status_ID" });
            DropIndex("dbo.Requests", new[] { "Computer_ID" });
            DropIndex("dbo.Computers", new[] { "Owner_ID" });
            DropIndex("dbo.ComponentTypes", new[] { "Category_ID" });
            DropIndex("dbo.Components", new[] { "Computer_ID" });
            DropIndex("dbo.Components", new[] { "Type_ID" });
            DropIndex("dbo.Calls", new[] { "Status_ID" });
            DropTable("dbo.Requests");
            DropTable("dbo.Customers");
            DropTable("dbo.Computers");
            DropTable("dbo.ComponentTypeCategories");
            DropTable("dbo.ComponentTypes");
            DropTable("dbo.Components");
            DropTable("dbo.Status");
            DropTable("dbo.Calls");
        }
    }
}
