namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeBridgeAnimeModel_NewAnimeReport : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NewAnimeReportAnimeModels", "NewAnimeReport_Id", "dbo.NewAnimeReports");
            DropForeignKey("dbo.NewAnimeReportAnimeModels", "AnimeModel_Id", "dbo.AnimeModels");
            DropIndex("dbo.NewAnimeReportAnimeModels", new[] { "NewAnimeReport_Id" });
            DropIndex("dbo.NewAnimeReportAnimeModels", new[] { "AnimeModel_Id" });
            AddColumn("dbo.AnimeModels", "NewAnimeReport_Id", c => c.Int());
            CreateIndex("dbo.AnimeModels", "NewAnimeReport_Id");
            AddForeignKey("dbo.AnimeModels", "NewAnimeReport_Id", "dbo.NewAnimeReports", "Id");
            DropTable("dbo.NewAnimeReportAnimeModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.NewAnimeReportAnimeModels",
                c => new
                    {
                        NewAnimeReport_Id = c.Int(nullable: false),
                        AnimeModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NewAnimeReport_Id, t.AnimeModel_Id });
            
            DropForeignKey("dbo.AnimeModels", "NewAnimeReport_Id", "dbo.NewAnimeReports");
            DropIndex("dbo.AnimeModels", new[] { "NewAnimeReport_Id" });
            DropColumn("dbo.AnimeModels", "NewAnimeReport_Id");
            CreateIndex("dbo.NewAnimeReportAnimeModels", "AnimeModel_Id");
            CreateIndex("dbo.NewAnimeReportAnimeModels", "NewAnimeReport_Id");
            AddForeignKey("dbo.NewAnimeReportAnimeModels", "AnimeModel_Id", "dbo.AnimeModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NewAnimeReportAnimeModels", "NewAnimeReport_Id", "dbo.NewAnimeReports", "Id", cascadeDelete: true);
        }
    }
}
