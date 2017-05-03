namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImproveEFmodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Calls", "Status_ID", "dbo.Status");
            DropForeignKey("dbo.Components", "Type_ID", "dbo.ComponentTypes");
            DropForeignKey("dbo.ComponentTypes", "Category_ID", "dbo.ComponentTypeCategories");
            DropForeignKey("dbo.Computers", "Owner_ID", "dbo.Customers");
            DropForeignKey("dbo.Requests", "Computer_ID", "dbo.Computers");
            DropForeignKey("dbo.Requests", "Status_ID", "dbo.Status");
            DropIndex("dbo.Calls", new[] { "Status_ID" });
            DropIndex("dbo.Components", new[] { "Type_ID" });
            DropIndex("dbo.ComponentTypes", new[] { "Category_ID" });
            DropIndex("dbo.Computers", new[] { "Owner_ID" });
            DropIndex("dbo.Requests", new[] { "Computer_ID" });
            DropIndex("dbo.Requests", new[] { "Status_ID" });
            RenameColumn(table: "dbo.Calls", name: "Status_ID", newName: "StatusID");
            RenameColumn(table: "dbo.Components", name: "Type_ID", newName: "TypeID");
            RenameColumn(table: "dbo.ComponentTypes", name: "Category_ID", newName: "CategoryID");
            RenameColumn(table: "dbo.Computers", name: "Owner_ID", newName: "OwnerID");
            RenameColumn(table: "dbo.Requests", name: "Computer_ID", newName: "ComputerID");
            RenameColumn(table: "dbo.Requests", name: "Status_ID", newName: "StatusID");
            AlterColumn("dbo.Calls", "StatusID", c => c.Int(nullable: false));
            AlterColumn("dbo.Components", "TypeID", c => c.Int(nullable: false));
            AlterColumn("dbo.ComponentTypes", "CategoryID", c => c.Int(nullable: false));
            AlterColumn("dbo.Computers", "OwnerID", c => c.Int(nullable: false));
            AlterColumn("dbo.Requests", "ReceivedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Requests", "ComputerID", c => c.Int(nullable: false));
            AlterColumn("dbo.Requests", "StatusID", c => c.Int(nullable: false));
            CreateIndex("dbo.Calls", "StatusID");
            CreateIndex("dbo.Components", "TypeID");
            CreateIndex("dbo.ComponentTypes", "CategoryID");
            CreateIndex("dbo.Computers", "OwnerID");
            CreateIndex("dbo.Requests", "ComputerID");
            CreateIndex("dbo.Requests", "StatusID");
            AddForeignKey("dbo.Calls", "StatusID", "dbo.Status", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Components", "TypeID", "dbo.ComponentTypes", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ComponentTypes", "CategoryID", "dbo.ComponentTypeCategories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Computers", "OwnerID", "dbo.Customers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "ComputerID", "dbo.Computers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "StatusID", "dbo.Status", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "StatusID", "dbo.Status");
            DropForeignKey("dbo.Requests", "ComputerID", "dbo.Computers");
            DropForeignKey("dbo.Computers", "OwnerID", "dbo.Customers");
            DropForeignKey("dbo.ComponentTypes", "CategoryID", "dbo.ComponentTypeCategories");
            DropForeignKey("dbo.Components", "TypeID", "dbo.ComponentTypes");
            DropForeignKey("dbo.Calls", "StatusID", "dbo.Status");
            DropIndex("dbo.Requests", new[] { "StatusID" });
            DropIndex("dbo.Requests", new[] { "ComputerID" });
            DropIndex("dbo.Computers", new[] { "OwnerID" });
            DropIndex("dbo.ComponentTypes", new[] { "CategoryID" });
            DropIndex("dbo.Components", new[] { "TypeID" });
            DropIndex("dbo.Calls", new[] { "StatusID" });
            AlterColumn("dbo.Requests", "StatusID", c => c.Int());
            AlterColumn("dbo.Requests", "ComputerID", c => c.Int());
            AlterColumn("dbo.Requests", "ReceivedDate", c => c.DateTime());
            AlterColumn("dbo.Computers", "OwnerID", c => c.Int());
            AlterColumn("dbo.ComponentTypes", "CategoryID", c => c.Int());
            AlterColumn("dbo.Components", "TypeID", c => c.Int());
            AlterColumn("dbo.Calls", "StatusID", c => c.Int());
            RenameColumn(table: "dbo.Requests", name: "StatusID", newName: "Status_ID");
            RenameColumn(table: "dbo.Requests", name: "ComputerID", newName: "Computer_ID");
            RenameColumn(table: "dbo.Computers", name: "OwnerID", newName: "Owner_ID");
            RenameColumn(table: "dbo.ComponentTypes", name: "CategoryID", newName: "Category_ID");
            RenameColumn(table: "dbo.Components", name: "TypeID", newName: "Type_ID");
            RenameColumn(table: "dbo.Calls", name: "StatusID", newName: "Status_ID");
            CreateIndex("dbo.Requests", "Status_ID");
            CreateIndex("dbo.Requests", "Computer_ID");
            CreateIndex("dbo.Computers", "Owner_ID");
            CreateIndex("dbo.ComponentTypes", "Category_ID");
            CreateIndex("dbo.Components", "Type_ID");
            CreateIndex("dbo.Calls", "Status_ID");
            AddForeignKey("dbo.Requests", "Status_ID", "dbo.Status", "ID");
            AddForeignKey("dbo.Requests", "Computer_ID", "dbo.Computers", "ID");
            AddForeignKey("dbo.Computers", "Owner_ID", "dbo.Customers", "ID");
            AddForeignKey("dbo.ComponentTypes", "Category_ID", "dbo.ComponentTypeCategories", "ID");
            AddForeignKey("dbo.Components", "Type_ID", "dbo.ComponentTypes", "ID");
            AddForeignKey("dbo.Calls", "Status_ID", "dbo.Status", "ID");
        }
    }
}
