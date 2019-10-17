namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAnimeModel_w_AdjustedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "AdjustedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnimeModels", "AdjustedDate");
        }
    }
}
