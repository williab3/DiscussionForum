namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComment_w_AddRepliedTo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "RepliedTo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "RepliedTo");
        }
    }
}
