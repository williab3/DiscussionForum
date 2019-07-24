namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser_w_LoginName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LoginName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LoginName");
        }
    }
}
