namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chengefileclass : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Files", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "Type", c => c.Int(nullable: false));
        }
    }
}
