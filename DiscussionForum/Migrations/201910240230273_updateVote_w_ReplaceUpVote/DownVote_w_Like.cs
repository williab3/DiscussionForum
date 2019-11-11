namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVote_w_ReplaceUpVoteDownVote_w_Like : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "Like", c => c.Boolean(nullable: false));
            DropColumn("dbo.Votes", "UpVote");
            DropColumn("dbo.Votes", "DownVote");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Votes", "DownVote", c => c.Boolean(nullable: false));
            AddColumn("dbo.Votes", "UpVote", c => c.Boolean(nullable: false));
            DropColumn("dbo.Votes", "Like");
        }
    }
}
