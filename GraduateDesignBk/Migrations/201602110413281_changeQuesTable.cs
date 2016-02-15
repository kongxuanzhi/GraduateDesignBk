namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeQuesTable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Bars");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Bars",
                c => new
                    {
                        BID = c.String(nullable: false, maxLength: 128),
                        FromUID = c.String(maxLength: 108),
                        ToUID = c.String(maxLength: 128),
                        FBID = c.String(maxLength: 128),
                        PBID = c.String(maxLength: 128),
                        Pub = c.Boolean(nullable: false),
                        RaiseQuesTime = c.DateTime(nullable: false),
                        Title = c.String(maxLength: 200),
                        Description = c.String(maxLength: 500),
                        Likes = c.Int(nullable: false),
                        Solved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BID);
            
        }
    }
}
