namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserToCall : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Calls", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Calls", "UserID");
            AddForeignKey("dbo.Calls", "UserID", "dbo.AppUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Calls", "UserID", "dbo.AppUsers");
            DropIndex("dbo.Calls", new[] { "UserID" });
            DropColumn("dbo.Calls", "UserID");
        }
    }
}
