namespace Helpdesk.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBasicCustomerStructure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Computers", "Owner_ID", c => c.Int());
            AddColumn("dbo.Requests", "ReceiverUser_ID", c => c.Int());
            AddColumn("dbo.Requests", "ResolverUser_ID", c => c.Int());
            CreateIndex("dbo.Computers", "Owner_ID");
            CreateIndex("dbo.Requests", "ReceiverUser_ID");
            CreateIndex("dbo.Requests", "ResolverUser_ID");
            AddForeignKey("dbo.Computers", "Owner_ID", "dbo.Customers", "ID");
            AddForeignKey("dbo.Requests", "ReceiverUser_ID", "dbo.Users", "ID");
            AddForeignKey("dbo.Requests", "ResolverUser_ID", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "ResolverUser_ID", "dbo.Users");
            DropForeignKey("dbo.Requests", "ReceiverUser_ID", "dbo.Users");
            DropForeignKey("dbo.Computers", "Owner_ID", "dbo.Customers");
            DropIndex("dbo.Requests", new[] { "ResolverUser_ID" });
            DropIndex("dbo.Requests", new[] { "ReceiverUser_ID" });
            DropIndex("dbo.Computers", new[] { "Owner_ID" });
            DropColumn("dbo.Requests", "ResolverUser_ID");
            DropColumn("dbo.Requests", "ReceiverUser_ID");
            DropColumn("dbo.Computers", "Owner_ID");
            DropTable("dbo.Users");
            DropTable("dbo.Customers");
        }
    }
}
