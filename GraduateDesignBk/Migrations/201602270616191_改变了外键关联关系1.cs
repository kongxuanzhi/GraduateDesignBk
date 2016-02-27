namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 改变了外键关联关系1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "FromUID", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "FromUID" });
            AlterColumn("dbo.Files", "FromUID", c => c.String(maxLength: 120));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Files", "FromUID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Files", "FromUID");
            AddForeignKey("dbo.Files", "FromUID", "dbo.AspNetUsers", "Id");
        }
    }
}
