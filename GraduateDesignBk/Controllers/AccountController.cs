using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GraduateDesignBk.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using RecruCol;

namespace GraduateDesignBk.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        #region 初始化获得managers

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public ApplicationDbContext _contextManger;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
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
            private set
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
        #endregion

        public ActionResult List(SearchAndPage UsersModel)
        {
            int pageSize = (int)UsersModel.PageSize+8;
            //查询所有
            UsersModel.UserItems = UserManager.Users.Select(m =>
            new UserViewModel()
            {
                Id = m.Id,
                UserName = m.UserName,  //一卡通号
                Photo = m.Photo,
                mayjor = m.Mayjor,
                level = m.level,
                Comment = m.Comment,
                RealName = m.RealName,  //真实姓名
            }).ToList();
            //条件筛选
            UsersModel.UserItems = UsersModel.UserItems.Where(
            m => m.RealName.Contains(CNTS(UsersModel.SerachRealName))
              && m.UserName.Contains(CNTS(UsersModel.SearchName))
            ).ToList();
            if(UsersModel.SearchMayor!=0)
                UsersModel.UserItems = UsersModel.UserItems.Where(m=> m.mayjor == (UsersModel.SearchMayor)).ToList();
            if (UsersModel.SearchLevel != 0)
                UsersModel.UserItems = UsersModel.UserItems.Where(m => m.level == (UsersModel.SearchLevel)).ToList();
            if (UsersModel.type == "UnGroupList")
            {
                UsersModel.UserItems = UsersModel.UserItems.Where(s => UserManager.GetRoles(s.Id).Count() == 0).ToList();
            }
            else
            {
                UsersModel.UserItems = UsersModel.UserItems.Where(s => UserManager.GetRoles(s.Id).Count() > 0).ToList();
                UsersModel.UserItems = UsersModel.UserItems.Where(s => UserManager.GetRoles(s.Id).FirstOrDefault().Equals(UsersModel.userType)).ToList();
            }
            UsersModel.TotalCount = UsersModel.UserItems.Count();
            UsersModel.PageNum = (int)Math.Ceiling((double)UsersModel.TotalCount / pageSize);
            //分页
            UsersModel.UserItems = UsersModel.UserItems.OrderBy(m => m.Id).Skip((UsersModel.CurIndex - 1) * pageSize).Take(pageSize).ToList();
            return View(UsersModel);
        }

        public string CNTS(string value)
        {
            return value == null ? "" : value;
        }

        public ActionResult AddNewUser(string roleType)
        {
            ViewData["RoleType"] = roleType;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewUser(AddNewUserModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["RoleType"] = model.RoleType;
                return View(model);
            }
            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.UserName,
                RealName = model.RealName,
                Mayjor = model.mayjor,
                level = model.level,
            };
            user.Comment = string.IsNullOrEmpty(model.Comment) ? user.Comment : model.Comment;
            IdentityResult result1 =   UserManager.Create(user, model.UserName);
            if (!result1.Succeeded)
            {
                ModelState.AddModelError("", "一卡通号已经注册");
                ViewData["RoleType"] = model.RoleType;
                return View(model);
            }
            IdentityResult result2 =  UserManager.AddToRole(user.Id,model.RoleType);
            if (result2.Succeeded)
            {
                //return RedirectToAction("List", new { userType =model.RoleType});
            }
            return View();
        }

        public ActionResult SelfCenter(string Id,string userType)
        {
            PersonInfo personInfo = UserManager.Users.Select(m =>
               new PersonInfo()
               {
                   Id = m.Id,
                   UserName = m.UserName,
                   RealName = m.RealName,
                   Photo = m.Photo,
                   Email = m.Email,
                   PhoneNumber = m.PhoneNumber,
                   mayjor = m.Mayjor,
                   level = m.level,
                   Comment = m.Comment
               }
            ).Where(m => m.Id == Id).First();
            personInfo.userType = userType;
            return View(personInfo);
        }
        
        public ActionResult SelfBars(string Id, string userType)
        {
            
            PersonBars personBars = new PersonBars();
            personBars.Id = Id;    personBars.userType = userType;
            personBars.pbars = ContextManger.Database.SqlQuery<BarDetail>("Select * from V_Bars_Users vb where vb.ToUID is null and vb.PBID=vb.FBID and  vb.FBID = '0'").Where(m=>m.FromId==Id).ToList();
            personBars.fbars = ContextManger.Database.SqlQuery<BarDetail>("Select * from V_Bars_Users vb where  vb.PBID=vb.FBID and  vb.FBID <> '0'").ToList();
            personBars.sbars = ContextManger.Database.SqlQuery<BarDetail>("Select * from V_Bars_Users vb where  vb.PBID <>vb.FBID").ToList();
            return View(personBars);
        }

        public ActionResult SelfFiles(string Id, string userType)
        {
            List<File> files = ContextManger.File.Where(m => m.FromUID == Id).ToList();
            return View(files);
        }

        public ActionResult SelfMsgs(string Id, string userType)
        {
            List<Notice> msgs = ContextManger.Notice.Where(m => m.FromUID == Id).ToList();
            return View(msgs);
        }

        public ActionResult stuOrTeachers(string Id, string userType)
        {
            //List<string> Ids = new List<string>();
            //if (userType.Equals("教师"))
            //{
            //    Ids = ContextManger.StuMentor.Where(m => m.TeacherUID == Id).Select(m => m.StudentUID).ToList();
            //}
            //else if (userType.Equals("学生"))
            //{
            //    Ids = ContextManger.StuMentor.Where(m => m.StudentUID == Id).Select(m => m.TeacherUID).ToList();
            //}
            //List<UserViewModel> stuOrTeaches = UserManager.Users.Select(m =>
            //       new UserViewModel()
            //       {
            //           Id = m.Id,
            //           UserName = m.UserName,  //一卡通号
            //           Photo = m.Photo,
            //           mayjor = m.Mayjor,
            //           level = m.level,
            //           Comment = m.Comment,
            //           RealName = m.RealName,  //真实姓名
            //       }).Where(m => Ids.Contains(m.Id)).ToList();
            //return View(stuOrTeaches);
            return View();
        }

        public void MyProperty(string Id, string userType) {
            UserDetailModel userDetail = new UserDetailModel();
            //根据id得到个人信息（AspNetUser表） 返回ApsNetUser对象
            userDetail.personInfo = UserManager.Users.Select(m =>
               new PersonInfo()
               {
                   Id = m.Id,
                   UserName = m.UserName,
                   RealName = m.RealName,
                   Photo = m.Photo,
                   Email = m.Email,
                   PhoneNumber = m.PhoneNumber,
                   mayjor = m.Mayjor,
                   level = m.level,
                   Comment = m.Comment
               }
           ).Where(m => m.Id == Id).First();
            //根据Id获得发布的帖子 Bars  分页 不查询
            userDetail.bars = ContextManger.Bars.Where(m => m.FromUID == Id).ToList();
            //根据Id获得上传的文件//Files  分页不查询
            userDetail.files = ContextManger.File.Where(m => m.FromUID == Id).ToList();
            //根据Id获得消息列表    分页不查询
            userDetail.msgs = ContextManger.Notice.Where(m => m.FromUID == Id).ToList();
            //根据Id获得该师(生)的学生(知道老师列表)  //分页 查询
            List<string> Ids = new List<string>();
            if (userType.Equals("教师"))
            {
                Ids = ContextManger.StuMentor.Where(m => m.TeacherUID == Id).Select(m => m.StudentUID).ToList();
            }
            else if (userType.Equals("学生"))
            {
                Ids = ContextManger.StuMentor.Where(m => m.StudentUID == Id).Select(m => m.TeacherUID).ToList();
            }
            userDetail.stuOrTeaches = UserManager.Users.Select(m =>
                   new UserViewModel()
                   {
                       Id = m.Id,
                       UserName = m.UserName,  //一卡通号
                       Photo = m.Photo,
                       mayjor = m.Mayjor,
                       level = m.level,
                       Comment = m.Comment,
                       RealName = m.RealName,  //真实姓名
                   }).Where(m => Ids.Contains(m.Id)).ToList();
        }

        [HttpPost]
        public async Task<ActionResult> AddUserToRole(string userIds,string roleName)
        {
            string[] id = userIds.Split(new char[] { '|' });
            for (int i = 0; i < id.Length; i++)
            {
                IdentityResult result = await UserManager.AddToRoleAsync(id[i], roleName);
                if (!result.Succeeded)
                {
                    return View("Error", new string[] { "加入失败" });
                }
            }
            return View("_SuccessView", new string[] { "加入成功" });
        }
        [HttpPost]
        public async Task<ActionResult> DeleteUser(string Ids,string RoleName)
        {
            string[] id = Ids.Split(new char[] { '|' });
            for (int i = 0; i < id.Length; i++)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(id[i]);
                if (user == null)
                {
                    return View("Error", new string[] { "没有该用户" });
                }
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return View("Error", new string[] { "删除失败" });
                }
            }
            return RedirectToAction("List", new { userType = RoleName });
        }
        //===========================================================
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
               // case SignInStatus.InvalidEmail:

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    //For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    //Send an email with this link

                    //    The GenerateEmailConfirmationTokenAsync method creates a secure confirmation 
                    //token and stores it in the ASP.NET Identity data store. The Url.Action method 
                    //creates a link containing the UserId and confirmation token.This link is then 
                    //emailed to the user, the user can click on the link in their email app to confirm
                    //their account.
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}