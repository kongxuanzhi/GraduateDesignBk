namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlevelToStudent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "level", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Comment", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Comment");
            DropColumn("dbo.AspNetUsers", "level");
        }
    }
}
