namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNewAnimeReport_w_AnilitIdsArray : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NewAnimeReports", "AnilistId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NewAnimeReports", "AnilistId", c => c.Int(nullable: false));
        }
    }
}
