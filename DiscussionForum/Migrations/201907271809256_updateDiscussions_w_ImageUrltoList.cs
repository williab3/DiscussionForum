namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDiscussions_w_ImageUrltoList : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Discussions", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Discussions", "ImageUrl", c => c.String());
        }
    }
}
