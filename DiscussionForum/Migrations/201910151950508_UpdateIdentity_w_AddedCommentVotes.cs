namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIdentity_w_AddedCommentVotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Votes", "ApplicationUser_Id");
            AddForeignKey("dbo.Votes", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Votes", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Votes", "ApplicationUser_Id");
        }
    }
}
