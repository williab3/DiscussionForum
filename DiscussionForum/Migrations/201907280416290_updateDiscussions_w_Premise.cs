namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDiscussions_w_Premise : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discussions", "Premise", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Discussions", "Premise");
        }
    }
}
