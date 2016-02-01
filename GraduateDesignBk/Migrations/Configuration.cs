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
            #region 用于统计用户的文件，信息，帖子和导师的个数
            //    string usp_count =
            //    "CREATE PROCEDURE usp_Count "+
            //   "@Id varchar(100), "+
            //   "@userType varchar(20) "+
            //   "AS "+
            //   "select "+
            //   "bars = (select count(*) from Bars where Bars.FromUID = @Id and Bars.PBID = '0'), "+
            //"files = (select count(*) from Files F where F.FromUID = @Id ),"+
            //"msgs = (select count(*) from Notices where FromUID = @Id ),"+
            //"stuOrNum = "+
            //   "(case @userType "+
            //   "when 'student'  Then(select count(*) from StuMentors where StudentUID = @Id ) "+
            //   "when 'teacher'  Then(select count(*) from StuMentors where TeacherUID = @Id ) "+
            //   "else 0 end) ";
            //    context.Database.ExecuteSqlCommand(usp_count);
            #endregion
            #region 删除用户时的触发器
            // string trig_delete_user = "CREATE TRIGGER trig_delete_users " +
            //"ON [dbo].[AspNetUsers] " +
            //"After DELETE " +
            //"AS " +
            //"BEGIN " +
            //    "delete from Bars where FromUID in (select id from deleted) or ToUID in (select id from deleted); " +
            //    "delete From Files where FromUID in (select id from deleted) " +
            //    "delete from Notices where FromUID in (select id from deleted) " +
            //    "delete from StuMentors where StudentUID in (select id from deleted) or TeacherUID in (select id from deleted); " +
            //    "delete from AspNetUserRoles where UserId in (select id from deleted) " +
            //    "delete from AspNetUserClaims where UserId in (select id from deleted) " +
            //    "delete from AspNetUserLogins where UserId in (select id from deleted) " +
            //    "END ";
            // context.Database.ExecuteSqlCommand(trig_delete_user);
            #endregion
            #region 从指定角色中移出某个用户时的触发器
            //string trig_remove_Usertype =
            //"CREATE TRIGGER [trig_remove_Usertype] " +
            //"ON [dbo].[AspNetUserRoles] " +
            //"after delete " +
            //"AS " +
            //"BEGIN " +
            //    "delete from StuMentors where StudentUID in (select USErID from deleted) or TeacherUID in  (select USErID from deleted)  " +
            //"END";
            //context.Database.ExecuteSqlCommand(trig_remove_Usertype);
            #endregion
            #region 删除文件时，同时删除下载上传信息
            //string trig_Delete_File =
            //"CREATE TRIGGER [trig_Delete_File] " +
            //"ON [dbo].[Files] " +
            //"after DELETE " +
            //"AS " +
            //"BEGIN " +
            //    "delete from DownUploads where FID in (select FID from deleted) " +
            //"END";
            //context.Database.ExecuteSqlCommand(trig_Delete_File);
            #endregion
            #region 删除消息时，同时删除接收和发送消息的信息
            //string trig_Delete_msgs = 
            //"CREATE TRIGGER [trig_Delete_msgs] " +
            //"ON [dbo].[Notices] " +
            //"after DELETE " +
            //"AS " +
            //"BEGIN " +
            //"delete from MassMegs where MID in (select MID from deleted) " +
            //"END ";
            //context.Database.ExecuteSqlCommand(trig_Delete_msgs);
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
