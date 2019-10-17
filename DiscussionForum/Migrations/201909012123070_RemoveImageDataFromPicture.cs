namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveImageDataFromPicture : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pictures", "ImageData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pictures", "ImageData", c => c.String());
        }
    }
}
