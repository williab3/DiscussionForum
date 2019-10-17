namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDiscussionTitleMaxLenth_140 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Discussions", "Title", c => c.String(nullable: false, maxLength: 140));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Discussions", "Title", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
