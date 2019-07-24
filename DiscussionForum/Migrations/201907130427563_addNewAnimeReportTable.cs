namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewAnimeReportTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewAnimeReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnilistId = c.Int(nullable: false),
                        InfoUrl = c.String(),
                        Title = c.String(),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NewAnimeReports");
        }
    }
}
