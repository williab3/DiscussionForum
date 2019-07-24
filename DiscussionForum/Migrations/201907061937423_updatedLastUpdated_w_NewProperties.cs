namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedLastUpdated_w_NewProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeStatsModels", "NextUpdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AnimeStatsModels", "AirTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnimeStatsModels", "AirTime");
            DropColumn("dbo.AnimeStatsModels", "NextUpdate");
        }
    }
}
