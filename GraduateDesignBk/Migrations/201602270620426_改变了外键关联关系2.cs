namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 改变了外键关联关系2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Files", "FromUID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Files", "FromUID");
            AddForeignKey("dbo.Files", "FromUID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "FromUID", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "FromUID" });
            AlterColumn("dbo.Files", "FromUID", c => c.String(maxLength: 120));
        }
    }
}
