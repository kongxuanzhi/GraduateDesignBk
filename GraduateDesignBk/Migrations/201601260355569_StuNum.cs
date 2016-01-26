namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StuNum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "StuNum", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "StuNum");
        }
    }
}
