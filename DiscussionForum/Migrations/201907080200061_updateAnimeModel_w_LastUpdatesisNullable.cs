namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAnimeModel_w_LastUpdatesisNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AnimeModels", "LastUpdated", c => c.DateTime());
            AlterColumn("dbo.Comments", "LastUpdated", c => c.DateTime());
            AlterColumn("dbo.Discussions", "LastUpdated", c => c.DateTime());
            AlterColumn("dbo.AnimeStatsModels", "LastUpdated", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AnimeStatsModels", "LastUpdated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Discussions", "LastUpdated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Comments", "LastUpdated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AnimeModels", "LastUpdated", c => c.DateTime(nullable: false));
        }
    }
}
