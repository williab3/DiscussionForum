namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePicture_removeImageUrl_addImageData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "ImageData", c => c.Binary());
            DropColumn("dbo.Pictures", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pictures", "ImageUrl", c => c.String());
            DropColumn("dbo.Pictures", "ImageData");
        }
    }
}
