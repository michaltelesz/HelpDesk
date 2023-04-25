namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserCreate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "UserID", "dbo.AppUsers");
            DropForeignKey("dbo.Consultants", "UserID", "dbo.AppUsers");
            DropIndex("dbo.Customers", new[] { "UserID" });
            DropIndex("dbo.Consultants", new[] { "UserID" });
            AlterColumn("dbo.Customers", "UserID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Consultants", "UserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Customers", "UserID");
            CreateIndex("dbo.Consultants", "UserID");
            AddForeignKey("dbo.Customers", "UserID", "dbo.AppUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Consultants", "UserID", "dbo.AppUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Consultants", "UserID", "dbo.AppUsers");
            DropForeignKey("dbo.Customers", "UserID", "dbo.AppUsers");
            DropIndex("dbo.Consultants", new[] { "UserID" });
            DropIndex("dbo.Customers", new[] { "UserID" });
            AlterColumn("dbo.Consultants", "UserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Customers", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Consultants", "UserID");
            CreateIndex("dbo.Customers", "UserID");
            AddForeignKey("dbo.Consultants", "UserID", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Customers", "UserID", "dbo.AppUsers", "Id");
        }
    }
}
