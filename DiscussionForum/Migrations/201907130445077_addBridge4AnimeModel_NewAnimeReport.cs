namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBridge4AnimeModel_NewAnimeReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewAnimeReportAnimeModels",
                c => new
                    {
                        NewAnimeReport_Id = c.Int(nullable: false),
                        AnimeModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NewAnimeReport_Id, t.AnimeModel_Id })
                .ForeignKey("dbo.NewAnimeReports", t => t.NewAnimeReport_Id, cascadeDelete: true)
                .ForeignKey("dbo.AnimeModels", t => t.AnimeModel_Id, cascadeDelete: true)
                .Index(t => t.NewAnimeReport_Id)
                .Index(t => t.AnimeModel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewAnimeReportAnimeModels", "AnimeModel_Id", "dbo.AnimeModels");
            DropForeignKey("dbo.NewAnimeReportAnimeModels", "NewAnimeReport_Id", "dbo.NewAnimeReports");
            DropIndex("dbo.NewAnimeReportAnimeModels", new[] { "AnimeModel_Id" });
            DropIndex("dbo.NewAnimeReportAnimeModels", new[] { "NewAnimeReport_Id" });
            DropTable("dbo.NewAnimeReportAnimeModels");
        }
    }
}
