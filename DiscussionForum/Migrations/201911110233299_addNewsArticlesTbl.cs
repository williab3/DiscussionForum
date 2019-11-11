namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewsArticlesTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsArticles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PublishDate = c.DateTime(),
                        Title = c.String(),
                        Content = c.String(),
                        ArticleImage_Id = c.Int(),
                        PrimaryAnime_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.ArticleImage_Id)
                .ForeignKey("dbo.AnimeModels", t => t.PrimaryAnime_Id)
                .Index(t => t.ArticleImage_Id)
                .Index(t => t.PrimaryAnime_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsArticles", "PrimaryAnime_Id", "dbo.AnimeModels");
            DropForeignKey("dbo.NewsArticles", "ArticleImage_Id", "dbo.Pictures");
            DropIndex("dbo.NewsArticles", new[] { "PrimaryAnime_Id" });
            DropIndex("dbo.NewsArticles", new[] { "ArticleImage_Id" });
            DropTable("dbo.NewsArticles");
        }
    }
}
