namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAnimeModel_w_RomanjiTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "Title_English", c => c.String());
            AddColumn("dbo.AnimeModels", "Title_Romaji", c => c.String());
            DropColumn("dbo.AnimeModels", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnimeModels", "Title", c => c.String());
            DropColumn("dbo.AnimeModels", "Title_Romaji");
            DropColumn("dbo.AnimeModels", "Title_English");
        }
    }
}
