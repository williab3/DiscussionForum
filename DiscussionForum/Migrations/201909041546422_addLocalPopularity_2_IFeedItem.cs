namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLocalPopularity_2_IFeedItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "LocalPopularity", c => c.Int(nullable: false));
            AddColumn("dbo.Discussions", "LocalPopularity", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "LocalPopularity", c => c.Int(nullable: false));
            AddColumn("dbo.AnimeStatsModels", "LocalPopularity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnimeStatsModels", "LocalPopularity");
            DropColumn("dbo.Comments", "LocalPopularity");
            DropColumn("dbo.Discussions", "LocalPopularity");
            DropColumn("dbo.AnimeModels", "LocalPopularity");
        }
    }
}
