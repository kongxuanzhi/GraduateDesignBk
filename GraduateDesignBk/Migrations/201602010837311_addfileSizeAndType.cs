namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfileSizeAndType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Files", "Size", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Size");
            DropColumn("dbo.Files", "Type");
        }
    }
}
