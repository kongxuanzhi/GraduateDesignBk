namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDescription : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "Description", c => c.String(maxLength: 500));
        }
    }
}
