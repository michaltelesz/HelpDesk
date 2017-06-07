namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStatusesTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Status", newName: "Statuses");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Statuses", newName: "Status");
        }
    }
}
