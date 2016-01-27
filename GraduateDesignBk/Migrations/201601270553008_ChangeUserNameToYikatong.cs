namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserNameToYikatong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RealName", c => c.String(maxLength: 50));
            DropColumn("dbo.AspNetUsers", "StuNum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "StuNum", c => c.String(maxLength: 50));
            DropColumn("dbo.AspNetUsers", "RealName");
        }
    }
}
