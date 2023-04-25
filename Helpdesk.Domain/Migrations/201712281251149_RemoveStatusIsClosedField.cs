namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStatusIsClosedField : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Statuses", "IsClosed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Statuses", "IsClosed", c => c.Boolean(nullable: false));
        }
    }
}
