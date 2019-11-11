namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateIdentity_w_ProfilePic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProfilePic_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ProfilePic_Id");
            AddForeignKey("dbo.AspNetUsers", "ProfilePic_Id", "dbo.Pictures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ProfilePic_Id", "dbo.Pictures");
            DropIndex("dbo.AspNetUsers", new[] { "ProfilePic_Id" });
            DropColumn("dbo.AspNetUsers", "ProfilePic_Id");
        }
    }
}
