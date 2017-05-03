namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeysToComponents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Components", "Computer_ID", "dbo.Computers");
            DropIndex("dbo.Components", new[] { "Computer_ID" });
            RenameColumn(table: "dbo.Components", name: "Computer_ID", newName: "ComputerID");
            AlterColumn("dbo.Components", "ComputerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Components", "ComputerID");
            AddForeignKey("dbo.Components", "ComputerID", "dbo.Computers", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Components", "ComputerID", "dbo.Computers");
            DropIndex("dbo.Components", new[] { "ComputerID" });
            AlterColumn("dbo.Components", "ComputerID", c => c.Int());
            RenameColumn(table: "dbo.Components", name: "ComputerID", newName: "Computer_ID");
            CreateIndex("dbo.Components", "Computer_ID");
            AddForeignKey("dbo.Components", "Computer_ID", "dbo.Computers", "ID");
        }
    }
}
