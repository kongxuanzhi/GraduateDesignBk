namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加公告类 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        NTID = c.String(nullable: false, maxLength: 120),
                        Content = c.String(maxLength: 200),
                        FromUID = c.String(maxLength: 120),
                    })
                .PrimaryKey(t => t.NTID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notes");
        }
    }
}
