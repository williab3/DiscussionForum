namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVote_ReplaceVoterId_w_VoterUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Votes", "FK_dbo.Votes_dbo.AspNetUsers_ApplicationUser_Id");
            DropForeignKey("dbo.Votes", "Voter_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Votes", new[] { "Voter_Id" });
            AddColumn("dbo.Votes", "VoterUserId", c => c.String(nullable: false));
            DropColumn("dbo.Votes", "Voter_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Votes", "Voter_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Votes", "VoterUserId");
            CreateIndex("dbo.Votes", "Voter_Id");
            AddForeignKey("dbo.Votes", "Voter_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
