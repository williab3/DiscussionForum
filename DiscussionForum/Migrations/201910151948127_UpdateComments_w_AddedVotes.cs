namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComments_w_AddedVotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "Comment_Id", c => c.Int());
            CreateIndex("dbo.Votes", "Comment_Id");
            AddForeignKey("dbo.Votes", "Comment_Id", "dbo.Comments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "Comment_Id", "dbo.Comments");
            DropIndex("dbo.Votes", new[] { "Comment_Id" });
            DropColumn("dbo.Votes", "Comment_Id");
        }
    }
}
