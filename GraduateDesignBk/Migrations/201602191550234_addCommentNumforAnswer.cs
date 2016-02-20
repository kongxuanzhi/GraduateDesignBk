namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCommentNumforAnswer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "CommentNum", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "CommentNum");
        }
    }
}
