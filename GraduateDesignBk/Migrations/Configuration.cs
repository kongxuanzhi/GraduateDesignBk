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
            //string sql1 = "drop Index UserNameIndex on AspNetUsers";
           // string sql2 = "create unique index UnIn_StuNum on AspNetUsers(StuNum)";
            //context.Database.ExecuteSqlCommand(sql1);
            //context.Database.ExecuteSqlCommand(sql2);
            //在这里创建视图，索引，存储过程，唯一性约束等等
            //context.Database.ExecuteSqlCommand();
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
