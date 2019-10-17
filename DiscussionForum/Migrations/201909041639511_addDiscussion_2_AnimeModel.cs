namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDiscussion_2_AnimeModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "Discussion_Id", c => c.Int());
            CreateIndex("dbo.AnimeModels", "Discussion_Id");
            AddForeignKey("dbo.AnimeModels", "Discussion_Id", "dbo.Discussions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnimeModels", "Discussion_Id", "dbo.Discussions");
            DropIndex("dbo.AnimeModels", new[] { "Discussion_Id" });
            DropColumn("dbo.AnimeModels", "Discussion_Id");
        }
    }
}
