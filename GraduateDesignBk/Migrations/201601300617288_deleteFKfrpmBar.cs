namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteFKfrpmBar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bars", "FromUID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bars", "ToUID", "dbo.AspNetUsers");
            DropIndex("dbo.Bars", new[] { "FromUID" });
            DropIndex("dbo.Bars", new[] { "ToUID" });
            AlterColumn("dbo.Bars", "FromUID", c => c.String(maxLength: 108));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bars", "FromUID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Bars", "ToUID");
            CreateIndex("dbo.Bars", "FromUID");
            AddForeignKey("dbo.Bars", "ToUID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Bars", "FromUID", "dbo.AspNetUsers", "Id");
        }
    }
}
