namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImproveEntities : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requests", "ReadableID", c => c.String(maxLength: 12));
            CreateIndex("dbo.Requests", "ReadableID", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Requests", new[] { "ReadableID" });
            AlterColumn("dbo.Requests", "ReadableID", c => c.String());
        }
    }
}
