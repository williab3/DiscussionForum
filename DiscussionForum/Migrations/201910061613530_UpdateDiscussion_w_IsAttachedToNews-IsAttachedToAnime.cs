namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDiscussion_w_IsAttachedToNewsIsAttachedToAnime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discussions", "IsAttachedToAnime", c => c.Boolean(nullable: false));
            AddColumn("dbo.Discussions", "IsAttachedToNews", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Discussions", "IsAttachedToNews");
            DropColumn("dbo.Discussions", "IsAttachedToAnime");
        }
    }
}
