namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class extendPremiselengthTo2000CharacterMax : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Discussions", "Premise", c => c.String(nullable: false, maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Discussions", "Premise", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
