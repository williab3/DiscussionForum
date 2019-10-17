namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComments_w_CommenterId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "CommenterId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "CommenterId");
        }
    }
}
