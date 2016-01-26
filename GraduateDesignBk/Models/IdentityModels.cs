using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GraduateDesignBk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUser():base(){
            base.Id = Guid.NewGuid().ToString();
            Comment = "暂无评语";
            Photo = "/img/default.png";
        }
        [StringLength(128)]
        public string Photo { get; set; }

        public Mayjor Mayjor { get; set; }

        public Level  level { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }
       
        [StringLength(50)]
        public string StuNum { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser,string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public enum Level
    {
        系2011级,
        系2012级,
        系2013级,
        系2014级,
        系2015级,
        系2016级
    }

    public enum Mayjor
    {
        信息与计算科学,
        数学专业,
        社体,
        电科
    }
    public class ApplicationUserRole : IdentityUserRole<string>
    {
    }

    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
    }

    public class ApplicationUserClaim : IdentityUserClaim<string>
    {}

    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole() : base()
        {
            base.Id = Guid.NewGuid().ToString();
           CreateTime = DateTime.Now;
        }
        public ApplicationRole(string Name)
        {
            base.Name = Name;
        }
        [StringLength(200)]
        public string Description { get; set; }

        public DateTime CreateTime { get; set; }
        [StringLength(200)]
        public string WhoCreate { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {

        public DbSet<Bar> Bars { get; set; }
        public DbSet<DownUpload> DownUpload { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<MassMeg> MassMeg { get; set; }
        public DbSet<Notice> Notice { get; set; }
        public DbSet<StuMentor> StuMentor { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection"){}
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //  public System.Data.Entity.DbSet<GraduateDesignBk.Models.ApplicationUser> ApplicationUsers { get; set; }
        #region 数据库初始化，行不通
        //static ApplicationDbContext()
        //{
        //    Database.SetInitializer(new ApplicationDbInitializer());
        //}
        #endregion
        //Error：ASP.NET Identity - Multiple object sets per type are not supported
        //public System.Data.Entity.DbSet<GraduateDesignBk.Models.ApplicationUser> User { get; set; }
    }
    #region 数据库初始化，行不通
    //public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    //{
    //    protected override void Seed(ApplicationDbContext context)
    //    {
    //        InitializeIdentityForEF(context);
    //        base.Seed(context);
    //    }
    //    private static void InitializeIdentityForEF(ApplicationDbContext dbcontext)
    //    {
    //        var userManager = HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
    //        var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

    //        const string UserName = "xuan";
    //        const string PassWord = "xuan";
    //        const string RoleName = "Admin";


    //        var user = userManager.FindByName(UserName);
    //        if (user == null)
    //        {
    //            user = new ApplicationUser { UserName = UserName,Email = UserName };
    //            var result = userManager.Create(user, PassWord);
    //            result = userManager.SetLockoutEnabled(user.Id, false);
    //        }

    //        var Role = roleManager.FindByName(RoleName);
    //        if (Role == null)
    //        {
    //            Role = new ApplicationRole(RoleName);
    //            var result = roleManager.Create<ApplicationRole, string>(Role);
    //        }

    //        var rolesForUser = userManager.GetRoles(user.Id);
    //        if(!rolesForUser.Contains(RoleName))
    //        {
    //            var result = userManager.AddToRole(user.Id,RoleName);
    //        }
    //    }

    //}
    #endregion
}