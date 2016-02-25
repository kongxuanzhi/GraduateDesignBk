using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using GraduateDesignBk.Models;

namespace GraduateDesignBk
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);


            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/"),
                Provider = new CookieAuthenticationProvider
                {
                    #region SecurityStampValidator 安全戳的作用
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an 
                    //external login to your account.  
                    // When you change your security profile, a new security stamp is generated and 
                    //stored in the SecurityStamp field of the AspNetUsers table. Note, the
                    //SecurityStamp field is different from the security cookie. The security cookie
                    //is not stored in the AspNetUsers table (or anywhere else in the Identity DB). 
                    //The security cookie token is self-signed using DPAPI and is created with the 
                    //UserId, SecurityStamp and expiration time information.
                    // The cookie middleware checks the cookie on each request.
                    //The SecurityStampValidator method in the Startup class hits the DB and 
                    //checks security stamp periodically, as specified with the validateInterval.
                    //This only happens every 30 minutes (in our sample) unless you change your security 
                    //profile.The 30 minute interval was chosen to minimize trips to the database. 
                    //See my two-factor authentication tutorial for more details.
                    //Per the comments in the code, the UseCookieAuthentication method supports
                    //cookie authentication. The SecurityStamp field and associated code 
                    //provides an extra layer of security to your app, when you change your password,
                    //you will be logged out of the browser you logged in with
                    //.The SecurityStampValidator.OnValidateIdentity method enables the app to
                    //validate the security token when the user logs in, which is used when you 
                    //change a password or use the external login.This is needed to ensure that any 
                    //tokens (cookies) generated with the old password are invalidated.In the sample
                    //project, if you change the users password then a new token is generated for the
                    //user, any previous tokens are invalidated and the SecurityStamp field is updated.
                    #endregion
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}