namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLevelToStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Statuses", "Level", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Statuses", "Level");
        }
    }
}
