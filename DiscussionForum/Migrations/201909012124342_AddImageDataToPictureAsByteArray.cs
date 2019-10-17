namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageDataToPictureAsByteArray : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "ImageData", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pictures", "ImageData");
        }
    }
}
