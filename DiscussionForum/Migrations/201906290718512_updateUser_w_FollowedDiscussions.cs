namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser_w_FollowedDiscussions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discussions", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Discussions", "ApplicationUser_Id");
            AddForeignKey("dbo.Discussions", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Discussions", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Discussions", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Discussions", "ApplicationUser_Id");
        }
    }
}
