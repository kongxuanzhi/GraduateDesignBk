namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfileType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Type", c => c.String(maxLength: 80));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Type");
        }
    }
}
