namespace GraduateDesignBk.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bars",
                c => new
                    {
                        BID = c.String(nullable: false, maxLength: 128),
                        FromUID = c.String(maxLength: 128),
                        ToUID = c.String(maxLength: 128),
                        FBID = c.String(maxLength: 128),
                        PBID = c.String(maxLength: 128),
                        Public = c.Boolean(nullable: false),
                        RaiseQuesTime = c.DateTime(nullable: false),
                        AnswerTime = c.DateTime(nullable: false),
                        Content = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.BID)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUID)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUID)
                .Index(t => t.FromUID)
                .Index(t => t.ToUID);
            
            CreateTable(
                "dbo.DownUploads",
                c => new
                    {
                        DID = c.String(nullable: false, maxLength: 128),
                        Time = c.DateTime(nullable: false),
                        ToUID = c.String(maxLength: 128),
                        FID = c.String(maxLength: 128),
                        Readed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DID)
                .ForeignKey("dbo.Files", t => t.FID)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUID)
                .Index(t => t.ToUID)
                .Index(t => t.FID);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FID = c.String(nullable: false, maxLength: 128),
                        FileSeq = c.String(maxLength: 128),
                        Name = c.String(maxLength: 120),
                        Public = c.Boolean(nullable: false),
                        UploadTimes = c.DateTime(nullable: false),
                        FromUID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FID)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUID)
                .Index(t => t.FromUID);
            
            CreateTable(
                "dbo.MassMegs",
                c => new
                    {
                        MID = c.String(nullable: false, maxLength: 128),
                        NID = c.String(nullable: false, maxLength: 128),
                        ToUID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MID)
                .ForeignKey("dbo.Notices", t => t.NID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUID)
                .Index(t => t.NID)
                .Index(t => t.ToUID);
            
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        NID = c.String(nullable: false, maxLength: 128),
                        CreateTime = c.DateTime(nullable: false),
                        Detail = c.String(maxLength: 500),
                        FromUID = c.String(maxLength: 128),
                        Readed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.NID)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUID)
                .Index(t => t.FromUID);
            
            CreateTable(
                "dbo.StuMentors",
                c => new
                    {
                        SMID = c.String(nullable: false, maxLength: 128),
                        StudentUID = c.String(maxLength: 128),
                        TeacherUID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SMID)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentUID)
                .ForeignKey("dbo.AspNetUsers", t => t.TeacherUID)
                .Index(t => t.StudentUID)
                .Index(t => t.TeacherUID);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StuMentors", "TeacherUID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StuMentors", "StudentUID", "dbo.AspNetUsers");
            DropForeignKey("dbo.MassMegs", "ToUID", "dbo.AspNetUsers");
            DropForeignKey("dbo.MassMegs", "NID", "dbo.Notices");
            DropForeignKey("dbo.Notices", "FromUID", "dbo.AspNetUsers");
            DropForeignKey("dbo.DownUploads", "ToUID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Files", "FromUID", "dbo.AspNetUsers");
            DropForeignKey("dbo.DownUploads", "FID", "dbo.Files");
            DropForeignKey("dbo.Bars", "ToUID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bars", "FromUID", "dbo.AspNetUsers");
            DropIndex("dbo.StuMentors", new[] { "TeacherUID" });
            DropIndex("dbo.StuMentors", new[] { "StudentUID" });
            DropIndex("dbo.Notices", new[] { "FromUID" });
            DropIndex("dbo.MassMegs", new[] { "ToUID" });
            DropIndex("dbo.MassMegs", new[] { "NID" });
            DropIndex("dbo.Files", new[] { "FromUID" });
            DropIndex("dbo.DownUploads", new[] { "FID" });
            DropIndex("dbo.DownUploads", new[] { "ToUID" });
            DropIndex("dbo.Bars", new[] { "ToUID" });
            DropIndex("dbo.Bars", new[] { "FromUID" });
            DropTable("dbo.StuMentors");
            DropTable("dbo.Notices");
            DropTable("dbo.MassMegs");
            DropTable("dbo.Files");
            DropTable("dbo.DownUploads");
            DropTable("dbo.Bars");
        }
    }
}
