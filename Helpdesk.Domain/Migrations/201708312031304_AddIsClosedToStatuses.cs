namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsClosedToStatuses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Statuses", "IsClosed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Statuses", "IsClosed");
        }
    }
}
