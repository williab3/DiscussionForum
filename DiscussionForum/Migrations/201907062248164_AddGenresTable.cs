namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenresTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnimeGenres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GenreName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AnimeGenres");
        }
    }
}
