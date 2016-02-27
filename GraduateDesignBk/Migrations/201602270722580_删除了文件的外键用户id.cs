namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 删除了文件的外键用户id : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "FromUID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Mesgs", "FromUID", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "FromUID" });
            DropIndex("dbo.Mesgs", new[] { "FromUID" });
            AlterColumn("dbo.Files", "FromUID", c => c.String(maxLength: 120));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Files", "FromUID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Mesgs", "FromUID");
            CreateIndex("dbo.Files", "FromUID");
            AddForeignKey("dbo.Mesgs", "FromUID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Files", "FromUID", "dbo.AspNetUsers", "Id");
        }
    }
}
