namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateGenres_w_animes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnimeGenres", "AnimeModel_Id", "dbo.AnimeModels");
            DropIndex("dbo.AnimeGenres", new[] { "AnimeModel_Id" });
            CreateTable(
                "dbo.AnimeGenreAnimeModels",
                c => new
                    {
                        AnimeGenre_Id = c.Int(nullable: false),
                        AnimeModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AnimeGenre_Id, t.AnimeModel_Id })
                .ForeignKey("dbo.AnimeGenres", t => t.AnimeGenre_Id, cascadeDelete: true)
                .ForeignKey("dbo.AnimeModels", t => t.AnimeModel_Id, cascadeDelete: true)
                .Index(t => t.AnimeGenre_Id)
                .Index(t => t.AnimeModel_Id);
            
            DropColumn("dbo.AnimeGenres", "AnimeModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnimeGenres", "AnimeModel_Id", c => c.Int());
            DropForeignKey("dbo.AnimeGenreAnimeModels", "AnimeModel_Id", "dbo.AnimeModels");
            DropForeignKey("dbo.AnimeGenreAnimeModels", "AnimeGenre_Id", "dbo.AnimeGenres");
            DropIndex("dbo.AnimeGenreAnimeModels", new[] { "AnimeModel_Id" });
            DropIndex("dbo.AnimeGenreAnimeModels", new[] { "AnimeGenre_Id" });
            DropTable("dbo.AnimeGenreAnimeModels");
            CreateIndex("dbo.AnimeGenres", "AnimeModel_Id");
            AddForeignKey("dbo.AnimeGenres", "AnimeModel_Id", "dbo.AnimeModels", "Id");
        }
    }
}
