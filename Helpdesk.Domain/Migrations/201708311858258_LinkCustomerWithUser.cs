namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkCustomerWithUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Customers", "UserID");
            AddForeignKey("dbo.Customers", "UserID", "dbo.AppUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "UserID", "dbo.AppUsers");
            DropIndex("dbo.Customers", new[] { "UserID" });
            DropColumn("dbo.Customers", "UserID");
        }
    }
}
