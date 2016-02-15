namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加公告类发布时间 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notes", "Time");
        }
    }
}
