namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequirementOfCreatedByInRequest : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Requests", new[] { "CreatedByID" });
            AlterColumn("dbo.Requests", "CreatedByID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Requests", "CreatedByID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Requests", new[] { "CreatedByID" });
            AlterColumn("dbo.Requests", "CreatedByID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Requests", "CreatedByID");
        }
    }
}
