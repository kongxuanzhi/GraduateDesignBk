namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePublic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bars", "Pub", c => c.Boolean(nullable: false));
            AddColumn("dbo.Files", "Pub", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bars", "Public");
            DropColumn("dbo.Files", "Public");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "Public", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bars", "Public", c => c.Boolean(nullable: false));
            DropColumn("dbo.Files", "Pub");
            DropColumn("dbo.Bars", "Pub");
        }
    }
}
