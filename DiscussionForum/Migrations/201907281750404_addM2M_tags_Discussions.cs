namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addM2M_tags_Discussions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TagDiscussions",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Discussion_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Discussion_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Discussions", t => t.Discussion_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Discussion_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagDiscussions", "Discussion_Id", "dbo.Discussions");
            DropForeignKey("dbo.TagDiscussions", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagDiscussions", new[] { "Discussion_Id" });
            DropIndex("dbo.TagDiscussions", new[] { "Tag_Id" });
            DropTable("dbo.TagDiscussions");
        }
    }
}
