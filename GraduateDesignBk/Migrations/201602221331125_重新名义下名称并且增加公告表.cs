namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 重新名义下名称并且增加公告表 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Notices", newName: "Mesgs");
            CreateTable(
                "dbo.Announces",
                c => new
                    {
                        ANID = c.String(nullable: false, maxLength: 120),
                        Title = c.String(),
                        Content = c.String(),
                        FromUID = c.String(maxLength: 120),
                        Time = c.DateTime(nullable: false),
                        ReadTimes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ANID);
            
            AlterColumn("dbo.Notes", "Content", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notes", "Content", c => c.String(maxLength: 200));
            DropTable("dbo.Announces");
            RenameTable(name: "dbo.Mesgs", newName: "Notices");
        }
    }
}
