namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtitleformeg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notices", "Title", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
           
            
            DropColumn("dbo.Notices", "Title");
        }
    }
}
