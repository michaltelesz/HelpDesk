namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LooseCustomerToUserDependency : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "UserID", "dbo.AppUsers");
            DropIndex("dbo.Customers", new[] { "UserID" });
            AlterColumn("dbo.Customers", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Customers", "UserID");
            AddForeignKey("dbo.Customers", "UserID", "dbo.AppUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "UserID", "dbo.AppUsers");
            DropIndex("dbo.Customers", new[] { "UserID" });
            AlterColumn("dbo.Customers", "UserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Customers", "UserID");
            AddForeignKey("dbo.Customers", "UserID", "dbo.AppUsers", "Id", cascadeDelete: true);
        }
    }
}
