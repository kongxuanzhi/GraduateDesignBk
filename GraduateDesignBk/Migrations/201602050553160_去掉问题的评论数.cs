namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 去掉问题的评论数 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bars", "CommentNum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bars", "CommentNum", c => c.Int(nullable: false));
        }
    }
}
