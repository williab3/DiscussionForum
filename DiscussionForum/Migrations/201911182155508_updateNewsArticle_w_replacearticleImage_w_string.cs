namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNewsArticle_w_replacearticleImage_w_string : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NewsArticles", "ArticleImage_Id", "dbo.Pictures");
            DropIndex("dbo.NewsArticles", new[] { "ArticleImage_Id" });
            AddColumn("dbo.NewsArticles", "ArticleImage", c => c.String());
            DropColumn("dbo.NewsArticles", "ArticleImage_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NewsArticles", "ArticleImage_Id", c => c.Int());
            DropColumn("dbo.NewsArticles", "ArticleImage");
            CreateIndex("dbo.NewsArticles", "ArticleImage_Id");
            AddForeignKey("dbo.NewsArticles", "ArticleImage_Id", "dbo.Pictures", "Id");
        }
    }
}
