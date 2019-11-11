namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVote_w_Voter : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Votes", name: "ApplicationUser_Id", newName: "Voter_Id");
            RenameIndex(table: "dbo.Votes", name: "IX_ApplicationUser_Id", newName: "IX_Voter_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Votes", name: "IX_Voter_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Votes", name: "Voter_Id", newName: "ApplicationUser_Id");
        }
    }
}
