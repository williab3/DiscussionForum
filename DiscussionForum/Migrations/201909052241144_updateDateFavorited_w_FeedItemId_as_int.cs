namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDateFavorited_w_FeedItemId_as_int : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DateFavoriteds", "FeedItemId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DateFavoriteds", "FeedItemId", c => c.String());
        }
    }
}
