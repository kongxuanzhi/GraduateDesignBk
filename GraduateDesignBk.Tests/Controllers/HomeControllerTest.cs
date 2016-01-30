using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraduateDesignBk.Controllers;
using GraduateDesignBk.Models;
using Microsoft.AspNet.Identity.EntityFramework;


namespace GraduateDesignBk.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {

        ApplicationUserManager UserManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(new ApplicationDbContext()));
        ApplicationRoleManager RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole, string, ApplicationUserRole>(new ApplicationDbContext()));
        ApplicationDbContext DbContext = new ApplicationDbContext();

        [TestMethod]
        public void Index()
        {
           string a = new AccountController().CNTS(null);
            //Assert.Equals(a,"");
            Console.WriteLine(a);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Str()
        {
            string a = "/Home/Create";
            int b =  a.IndexOfAny(new char[]{ '/'});
            bool c = a.Contains("Home");
        }
    }
}
