namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeFileSizeType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Files", "Size");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "Size", c => c.Int(nullable: false));
        }
    }
}
