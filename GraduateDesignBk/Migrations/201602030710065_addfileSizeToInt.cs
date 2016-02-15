namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfileSizeToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Files", "Size", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Files", "Size", c => c.Double(nullable: false));
        }
    }
}
