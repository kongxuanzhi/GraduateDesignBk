using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using GraduateDesignBk.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace GraduateDesignBk.Controllers
{
    public class RoleController : Controller
    {
        // GET: /Role/Index
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
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
            mroles.Name = role.RoleName;
            mroles.Description = role.Description;
            mroles.WhoCreate = User.Identity.GetUserName();

            var result = await RoleManager.UpdateAsync(mroles);
            if (result.Succeeded)
            {
                return View("_SuccessView", new { Result = "修改成功" });
            }
            return View();
        }


        #region helper
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

        private ApplicationRoleManager _roleManager;
        #endregion
    }
}