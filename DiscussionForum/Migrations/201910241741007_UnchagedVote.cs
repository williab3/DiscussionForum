namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnchagedVote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "UpVote", c => c.Boolean(nullable: false));
            AddColumn("dbo.Votes", "DownVote", c => c.Boolean(nullable: false));
            DropColumn("dbo.Votes", "Like");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Votes", "Like", c => c.Boolean(nullable: false));
            DropColumn("dbo.Votes", "DownVote");
            DropColumn("dbo.Votes", "UpVote");
        }
    }
}
