namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 为问题增加标题和描述赞和评论数目 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bars", "Title", c => c.String(maxLength: 200));
            AddColumn("dbo.Bars", "Description", c => c.String(maxLength: 500));
            AddColumn("dbo.Bars", "Like", c => c.Int(nullable: false));
            AddColumn("dbo.Bars", "CommentNum", c => c.Int(nullable: false));
            AddColumn("dbo.Bars", "Solved", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bars", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bars", "Content", c => c.String(maxLength: 500));
            DropColumn("dbo.Bars", "Solved");
            DropColumn("dbo.Bars", "CommentNum");
            DropColumn("dbo.Bars", "Like");
            DropColumn("dbo.Bars", "Description");
            DropColumn("dbo.Bars", "Title");
        }
    }
}
