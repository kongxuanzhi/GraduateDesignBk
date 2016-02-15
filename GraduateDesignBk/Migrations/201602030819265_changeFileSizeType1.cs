namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeFileSizeType1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Size", c => c.String(maxLength: 80));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Size");
        }
    }
}
