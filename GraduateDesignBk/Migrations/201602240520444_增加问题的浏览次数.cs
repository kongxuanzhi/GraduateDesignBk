namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加问题的浏览次数 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "ReadTimes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "ReadTimes");
        }
    }
}
