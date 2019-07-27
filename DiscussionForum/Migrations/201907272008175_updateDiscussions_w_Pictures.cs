namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDiscussions_w_Pictures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "Discussion_Id", c => c.Int());
            CreateIndex("dbo.Pictures", "Discussion_Id");
            AddForeignKey("dbo.Pictures", "Discussion_Id", "dbo.Discussions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "Discussion_Id", "dbo.Discussions");
            DropIndex("dbo.Pictures", new[] { "Discussion_Id" });
            DropColumn("dbo.Pictures", "Discussion_Id");
        }
    }
}
