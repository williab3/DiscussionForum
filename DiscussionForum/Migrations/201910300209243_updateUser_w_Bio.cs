namespace DiscussionForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser_w_Bio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Bio", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Bio");
        }
    }
}
