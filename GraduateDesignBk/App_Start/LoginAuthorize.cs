using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace GraduateDesignBk.App_Start
{
    public class LoginAuthorizeAttribute : AuthorizeAttribute
    {
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    bool p = base.AuthorizeCore(httpContext);
        //    ApplicationUserManager UserManager =  httpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    IPrincipal user = httpContext.User;
        //    string path = httpContext.Request.Path;
        //    user.Identity.GetUserId();
        //    return this.Roles==;
        //}
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Write("document.getElementById('LoginModelBtn').click();");
            filterContext.HttpContext.Response.End();
             //filterContext.Result = new HttpUnauthorizedResult();
           base.HandleUnauthorizedRequest(filterContext);
        }
    }
 
}
