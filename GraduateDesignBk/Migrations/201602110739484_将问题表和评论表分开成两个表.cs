namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 将问题表和评论表分开成两个表 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AID = c.String(nullable: false, maxLength: 128),
                        FromUID = c.String(maxLength: 108),
                        ToUID = c.String(maxLength: 128),
                        FAID = c.String(maxLength: 128),
                        PQID = c.String(maxLength: 128),
                        AnswerQuesTime = c.DateTime(nullable: false),
                        Description = c.String(maxLength: 500),
                        Likes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QID = c.String(nullable: false, maxLength: 128),
                        FromUID = c.String(maxLength: 108),
                        ToUID = c.String(maxLength: 128),
                        Pub = c.Boolean(nullable: false),
                        RaiseQuesTime = c.DateTime(nullable: false),
                        Title = c.String(maxLength: 200),
                        Description = c.String(maxLength: 500),
                        Likes = c.Int(nullable: false),
                        Solved = c.Boolean(nullable: false),
                        CommentNum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
