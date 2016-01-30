namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addViewForbars : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bars", "AnswerTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bars", "AnswerTime", c => c.DateTime(nullable: false));
        }
    }
}
