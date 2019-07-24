namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAnimeModel_w_ImageUrl_Sizes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "ImageUrlLarge", c => c.String());
            AddColumn("dbo.AnimeModels", "ImageUrlMedium", c => c.String());
            DropColumn("dbo.AnimeModels", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnimeModels", "ImageUrl", c => c.String());
            DropColumn("dbo.AnimeModels", "ImageUrlMedium");
            DropColumn("dbo.AnimeModels", "ImageUrlLarge");
        }
    }
}
