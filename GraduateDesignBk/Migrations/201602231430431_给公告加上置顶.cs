namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 给公告加上置顶 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announces", "Prop", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announces", "Prop");
        }
    }
}
