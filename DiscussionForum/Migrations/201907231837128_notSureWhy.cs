namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notSureWhy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeModels", "IsFavorite", c => c.Boolean());
            AddColumn("dbo.AnimeModels", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnimeModels", "Discriminator");
            DropColumn("dbo.AnimeModels", "IsFavorite");
        }
    }
}
