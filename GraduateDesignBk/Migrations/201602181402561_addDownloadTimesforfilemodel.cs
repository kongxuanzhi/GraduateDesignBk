namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDownloadTimesforfilemodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "DownloadTimes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "DownloadTimes");
        }
    }
}
