namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssignedUsersToRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "CreatedByID", c => c.String(maxLength: 128));
            AddColumn("dbo.Requests", "ClosedByID", c => c.Int());
            AddColumn("dbo.Requests", "AssignedToID", c => c.Int());
            CreateIndex("dbo.Requests", "CreatedByID");
            CreateIndex("dbo.Requests", "ClosedByID");
            CreateIndex("dbo.Requests", "AssignedToID");
            AddForeignKey("dbo.Requests", "AssignedToID", "dbo.Consultants", "ID");
            AddForeignKey("dbo.Requests", "ClosedByID", "dbo.Consultants", "ID");
            AddForeignKey("dbo.Requests", "CreatedByID", "dbo.AppUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "CreatedByID", "dbo.AppUsers");
            DropForeignKey("dbo.Requests", "ClosedByID", "dbo.Consultants");
            DropForeignKey("dbo.Requests", "AssignedToID", "dbo.Consultants");
            DropIndex("dbo.Requests", new[] { "AssignedToID" });
            DropIndex("dbo.Requests", new[] { "ClosedByID" });
            DropIndex("dbo.Requests", new[] { "CreatedByID" });
            DropColumn("dbo.Requests", "AssignedToID");
            DropColumn("dbo.Requests", "ClosedByID");
            DropColumn("dbo.Requests", "CreatedByID");
        }
    }
}
