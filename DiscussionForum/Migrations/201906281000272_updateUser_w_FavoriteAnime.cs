namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser_w_FavoriteAnime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AnimeModels", "ApplicationUser_Id");
            AddForeignKey("dbo.AnimeModels", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnimeModels", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AnimeModels", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AnimeModels", "ApplicationUser_Id");
        }
    }
}
