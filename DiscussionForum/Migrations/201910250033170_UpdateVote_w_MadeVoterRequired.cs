namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVote_w_MadeVoterRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Votes", "Voter_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Votes", new[] { "Voter_Id" });
            AlterColumn("dbo.Votes", "Voter_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Votes", "Voter_Id");
            AddForeignKey("dbo.Votes", "Voter_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "Voter_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Votes", new[] { "Voter_Id" });
            AlterColumn("dbo.Votes", "Voter_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Votes", "Voter_Id");
            AddForeignKey("dbo.Votes", "Voter_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
