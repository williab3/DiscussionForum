namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createAnimesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnimeModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnilistId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AnimeModels");
        }
    }
}
