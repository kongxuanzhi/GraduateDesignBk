namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加一个只能给问题点赞一次的表 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.likeOnces",
                c => new
                    {
                        LID = c.String(nullable: false, maxLength: 128),
                        BID = c.String(),
                        FromUID = c.String(),
                    })
                .PrimaryKey(t => t.LID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.likeOnces");
        }
    }
}
