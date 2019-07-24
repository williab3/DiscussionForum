namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAnimeModel_w_Genres : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeGenres", "AnimeModel_Id", c => c.Int());
            CreateIndex("dbo.AnimeGenres", "AnimeModel_Id");
            AddForeignKey("dbo.AnimeGenres", "AnimeModel_Id", "dbo.AnimeModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnimeGenres", "AnimeModel_Id", "dbo.AnimeModels");
            DropIndex("dbo.AnimeGenres", new[] { "AnimeModel_Id" });
            DropColumn("dbo.AnimeGenres", "AnimeModel_Id");
        }
    }
}
