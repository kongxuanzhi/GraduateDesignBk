namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moveReaded : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.MassMegs", "Readed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Notices", "Readed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notices", "Readed", c => c.Boolean(nullable: false));
            DropColumn("dbo.MassMegs", "Readed");
        }
    }
}
