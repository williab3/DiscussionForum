namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAnimeModel_w_StartdDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "StartDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnimeModels", "StartDate");
        }
    }
}
