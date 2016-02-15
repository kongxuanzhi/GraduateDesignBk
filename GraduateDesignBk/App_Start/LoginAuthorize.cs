using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GraduateDesignBk.App_Start
{
    public class LoginAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Write("document.getElementById('LoginModelBtn').click();");
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}
