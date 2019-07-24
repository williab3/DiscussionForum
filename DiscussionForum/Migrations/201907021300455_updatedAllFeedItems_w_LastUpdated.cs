namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedAllFeedItems_w_LastUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "LastUpdated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comments", "LastUpdated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Discussions", "LastUpdated", c => c.DateTime(nullable: false));
            DropColumn("dbo.Comments", "When");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "When", c => c.DateTime(nullable: false));
            DropColumn("dbo.Discussions", "LastUpdated");
            DropColumn("dbo.Comments", "LastUpdated");
            DropColumn("dbo.AnimeModels", "LastUpdated");
        }
    }
}
