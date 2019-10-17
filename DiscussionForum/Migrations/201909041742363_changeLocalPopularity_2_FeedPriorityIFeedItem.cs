namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeLocalPopularity_2_FeedPriorityIFeedItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "FeedPriority", c => c.Int(nullable: false));
            AddColumn("dbo.Discussions", "FeedPriority", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "FeedPriority", c => c.Int(nullable: false));
            AddColumn("dbo.AnimeStatsModels", "FeedPriority", c => c.Int(nullable: false));
            DropColumn("dbo.AnimeModels", "LocalPopularity");
            DropColumn("dbo.Discussions", "LocalPopularity");
            DropColumn("dbo.Comments", "LocalPopularity");
            DropColumn("dbo.AnimeStatsModels", "LocalPopularity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnimeStatsModels", "LocalPopularity", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "LocalPopularity", c => c.Int(nullable: false));
            AddColumn("dbo.Discussions", "LocalPopularity", c => c.Int(nullable: false));
            AddColumn("dbo.AnimeModels", "LocalPopularity", c => c.Int(nullable: false));
            DropColumn("dbo.AnimeStatsModels", "FeedPriority");
            DropColumn("dbo.Comments", "FeedPriority");
            DropColumn("dbo.Discussions", "FeedPriority");
            DropColumn("dbo.AnimeModels", "FeedPriority");
        }
    }
}
