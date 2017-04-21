namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComputerToRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Computer_ID", c => c.Int());
            CreateIndex("dbo.Requests", "Computer_ID");
            AddForeignKey("dbo.Requests", "Computer_ID", "dbo.Computers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "Computer_ID", "dbo.Computers");
            DropIndex("dbo.Requests", new[] { "Computer_ID" });
            DropColumn("dbo.Requests", "Computer_ID");
        }
    }
}
