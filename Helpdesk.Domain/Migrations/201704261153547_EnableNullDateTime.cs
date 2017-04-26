namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnableNullDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Calls", "Date", c => c.DateTime());
            AlterColumn("dbo.Requests", "ReceivedDate", c => c.DateTime());
            AlterColumn("dbo.Requests", "ResolvedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requests", "ResolvedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Requests", "ReceivedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Calls", "Date", c => c.DateTime(nullable: false));
        }
    }
}
