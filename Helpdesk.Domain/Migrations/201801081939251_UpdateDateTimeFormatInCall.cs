namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDateTimeFormatInCall : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Calls", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Calls", "Date", c => c.DateTime());
        }
    }
}
