namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComment_w_CommentorPic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "CommentorPic_Id", c => c.Int());
            CreateIndex("dbo.Comments", "CommentorPic_Id");
            AddForeignKey("dbo.Comments", "CommentorPic_Id", "dbo.Pictures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "CommentorPic_Id", "dbo.Pictures");
            DropIndex("dbo.Comments", new[] { "CommentorPic_Id" });
            DropColumn("dbo.Comments", "CommentorPic_Id");
        }
    }
}
