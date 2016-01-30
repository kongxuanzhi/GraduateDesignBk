using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using GraduateDesignBk.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RecruCol;
using System.Data.SqlClient;

namespace GraduateDesignBk.Controllers
{
    public class RoleController : Controller
    {
        // GET: /Role/Index
        public ActionResult Index()
        {
            List<ListRoleModel> roles = new List<ListRoleModel>();
            string sql = "select count(*) from AspNetUserRoles as r where r.RoleId= @roleID";  //
           // int RoleMemCount = ContextManger.Database.ExecuteSqlCommand(sql);
            foreach (var role in RoleManager.Roles.ToList())
            {
                roles.Add(new ListRoleModel() {
                    Id = role.Id,
                    Description = role.Description,
                    CreateTime = role.CreateTime,
                    WhoCreate = role.WhoCreate,
                    Name = role.Name,
                    RoleMemCount = (int)SqlHelper.ExecuteScalar(sql, new SqlParameter("@roleID", role.Id))
                    //UserManager.Users.Select(m => UserManager.IsInRole(m.Id, role.Name)).Count()
                });
            }
            return View(roles);
        }

        public bool IsInRole(string Id, string role)
        {
            return UserManager.IsInRole(Id,role);
        }
        //Get： /Role/Create 
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddNewRoleModel role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            var mrole = new ApplicationRole {
                Name = role.RoleName,
                Description = role.Description,
                WhoCreate = User.Identity.GetUserName()
            };
            var result = await RoleManager.CreateAsync(mrole);
            if (result.Succeeded)
            {
                return View("_SuccessView",new {Result = "创建成功" });
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string Id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(Id);
            if (role != null)
            {
                if (role.Name == "Admin")
                {
                    return View("Error",new string[] { "请勿删除管理员角色"});
                }
                IdentityResult result = await RoleManager.DeleteAsync(role);
                //删了角色，要触发所有该角色用户被角色被删除 UserRole表
                if (result.Succeeded)
                {
                    return RedirectToAction("Index","Role");
                }
                else
                {
                    return View("Error",result.Errors);
                }
            }
            return View("Error",new string[] { "无法找到该角色"});
        }

        public async Task<ActionResult> Detail(string Id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return View("Error", new string[] { "该角色不存在！" });
            }
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var user in UserManager.Users.ToList())
            {
                if(await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }
            ViewBag.Users = users;
            return View(role);
        }
        public async Task<ActionResult> Edit(string Id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return View("Error", new string[] { "该角色不存在！" });
            }
            ChangeRoleModel model = new ChangeRoleModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Description = role.Description
            };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ChangeRoleModel role)
        {
            ApplicationRole mroles = await RoleManager.FindByIdAsync(role.Id);
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            mroles.Id = role.Id;
            mroles.Description = role.Description;
            mroles.WhoCreate = User.Identity.GetUserName();

            var result = await RoleManager.UpdateAsync(mroles);
            if (result.Succeeded)
            {
                return View("_SuccessView", new { Result = "修改成功" });
            }
            return View();
        }
        [HttpPost]
        public async  Task<ActionResult> RemoveUserFromRole(string Id,string roleName)
        {
            string[] id = Id.Split(new char[] { '|' });
            for (int i = 0; i < id.Length; i++)
            {
                IdentityResult result = await UserManager.RemoveFromRoleAsync(id[i], roleName);
                if (!result.Succeeded)
                {
                    return View("Error", new string[] { "失败！" });
                }
            }
            return View("_SuccessView", new { Result = "移出成功" });
        }

        #region helper
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }

        public ApplicationDbContext ContextManger
        {
            get
            {
                return _contextManger ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _contextManger = value;
            }
        }
        public ApplicationDbContext _contextManger;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        #endregion
    }
}