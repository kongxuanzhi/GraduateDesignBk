namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeUploadTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "UploadTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Files", "UploadTimes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "UploadTimes", c => c.DateTime(nullable: false));
            DropColumn("dbo.Files", "UploadTime");
        }
    }
}
