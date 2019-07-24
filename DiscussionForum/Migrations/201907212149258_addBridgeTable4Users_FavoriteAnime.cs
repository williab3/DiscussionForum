namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBridgeTable4Users_FavoriteAnime : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnimeModels", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AnimeModels", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.ApplicationUserAnimeModels",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        AnimeModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.AnimeModel_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.AnimeModels", t => t.AnimeModel_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.AnimeModel_Id);
            
            DropColumn("dbo.AnimeModels", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnimeModels", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ApplicationUserAnimeModels", "AnimeModel_Id", "dbo.AnimeModels");
            DropForeignKey("dbo.ApplicationUserAnimeModels", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserAnimeModels", new[] { "AnimeModel_Id" });
            DropIndex("dbo.ApplicationUserAnimeModels", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserAnimeModels");
            CreateIndex("dbo.AnimeModels", "ApplicationUser_Id");
            AddForeignKey("dbo.AnimeModels", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
