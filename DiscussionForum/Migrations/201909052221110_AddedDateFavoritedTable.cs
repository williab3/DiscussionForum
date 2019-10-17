namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDateFavoritedTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DateFavoriteds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        FeedItemId = c.String(),
                        WhenFavorited = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DateFavoriteds");
        }
    }
}
