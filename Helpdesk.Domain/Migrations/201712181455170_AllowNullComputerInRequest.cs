namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNullComputerInRequest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "ComputerID", "dbo.Computers");
            DropIndex("dbo.Requests", new[] { "ComputerID" });
            AlterColumn("dbo.Requests", "ComputerID", c => c.Int());
            CreateIndex("dbo.Requests", "ComputerID");
            AddForeignKey("dbo.Requests", "ComputerID", "dbo.Computers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "ComputerID", "dbo.Computers");
            DropIndex("dbo.Requests", new[] { "ComputerID" });
            AlterColumn("dbo.Requests", "ComputerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Requests", "ComputerID");
            AddForeignKey("dbo.Requests", "ComputerID", "dbo.Computers", "ID", cascadeDelete: true);
        }
    }
}
