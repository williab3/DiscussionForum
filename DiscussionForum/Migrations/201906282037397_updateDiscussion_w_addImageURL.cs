namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDiscussion_w_addImageURL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discussions", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Discussions", "ImageUrl");
        }
    }
}
