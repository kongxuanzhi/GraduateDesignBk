namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReadStateEnumforread : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DownUploads", "Readstate", c => c.Int(nullable: false));
            AddColumn("dbo.MassMegs", "Readstate", c => c.Int(nullable: false));
            DropColumn("dbo.DownUploads", "Readed");
            DropColumn("dbo.MassMegs", "Readed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MassMegs", "Readed", c => c.Boolean(nullable: false));
            AddColumn("dbo.DownUploads", "Readed", c => c.Boolean(nullable: false));
            DropColumn("dbo.MassMegs", "Readstate");
            DropColumn("dbo.DownUploads", "Readstate");
        }
    }
}
