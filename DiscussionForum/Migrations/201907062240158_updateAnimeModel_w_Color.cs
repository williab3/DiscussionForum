namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAnimeModel_w_Color : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "Color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnimeModels", "Color");
        }
    }
}
