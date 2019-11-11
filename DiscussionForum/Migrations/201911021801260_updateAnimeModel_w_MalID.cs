namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAnimeModel_w_MalID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "MALId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnimeModels", "MALId");
        }
    }
}
