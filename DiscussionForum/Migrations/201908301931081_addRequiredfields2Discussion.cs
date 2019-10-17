namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequiredfields2Discussion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Discussions", "Title", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Discussions", "Premise", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Discussions", "Premise", c => c.String());
            AlterColumn("dbo.Discussions", "Title", c => c.String());
        }
    }
}
