namespace GraduateDesignBk.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.AspNet.Identity;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<GraduateDesignBk.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "GraduateDesignBk.Models.ApplicationDbContext";
        }

        protected override void Seed(GraduateDesignBk.Models.ApplicationDbContext context)
        {
            #region 为Bars添加一个视图，便于和user表关联
            //string delete = "drop view V_Bars_Users";
           // context.Database.ExecuteSqlCommand(delete);
            //string ViewForBars =
            //"CREATE VIEW [dbo].[V_Bars_Users] AS select B.BID, A1.RealName as FromUID ,A2.RealName ToUID, B.FBID, B.PBID, B.Pub, B.RaiseQuesTime,B.Content, " +
            //" A1.Photo as FromPhoto, A1.Id as FromId, A2.Id as ToId "+
            //"from Bars B " +
            //"left join AspNetUsers A1 on B.FromUID = A1.id " +
            //"left join AspNetUsers A2 on B.ToUID = A2.id";
            //context.Database.ExecuteSqlCommand(ViewForBars);
            #endregion

            #region 示例
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            #endregion
        }
    }
}
