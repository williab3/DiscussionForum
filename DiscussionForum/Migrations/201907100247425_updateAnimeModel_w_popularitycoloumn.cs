namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAnimeModel_w_popularitycoloumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "Popularity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnimeModels", "Popularity");
        }
    }
}
