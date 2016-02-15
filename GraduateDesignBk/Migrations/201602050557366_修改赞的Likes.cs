namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 修改赞的Likes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bars", "Likes", c => c.Int(nullable: false));
            DropColumn("dbo.Bars", "Like");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bars", "Like", c => c.Int(nullable: false));
            DropColumn("dbo.Bars", "Likes");
        }
    }
}
