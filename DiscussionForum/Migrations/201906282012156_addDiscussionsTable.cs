namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDiscussionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Who = c.String(),
                        What = c.String(),
                        When = c.DateTime(nullable: false),
                        Likes = c.Int(nullable: false),
                        Comment_Id = c.Int(),
                        Discussion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.Comment_Id)
                .ForeignKey("dbo.Discussions", t => t.Discussion_Id)
                .Index(t => t.Comment_Id)
                .Index(t => t.Discussion_Id);
            
            CreateTable(
                "dbo.Discussions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Discussion_Id", "dbo.Discussions");
            DropForeignKey("dbo.Comments", "Comment_Id", "dbo.Comments");
            DropIndex("dbo.Comments", new[] { "Discussion_Id" });
            DropIndex("dbo.Comments", new[] { "Comment_Id" });
            DropTable("dbo.Discussions");
            DropTable("dbo.Comments");
        }
    }
}
