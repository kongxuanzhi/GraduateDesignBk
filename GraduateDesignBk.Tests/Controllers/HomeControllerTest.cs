using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraduateDesignBk.Controllers;
using GraduateDesignBk.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace GraduateDesignBk.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {

        ApplicationUserManager UserManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(new ApplicationDbContext()));
        ApplicationRoleManager RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole, string, ApplicationUserRole>(new ApplicationDbContext()));


        [TestMethod]
        public void Index()
        {
            // SearchAndPage UsersModel = new SearchAndPage();
            // UsersModel.UserItems = new List<UserViewModel>();
            ////select * from AspNetUsers where 
            var temp = UserManager.Users.ToList();//Select(m =>
            //new UserViewModel()
            //{
            //    Id = m.Id,
            //    UserName = m.UserName,
            //    Photo = m.Photo,
            //    mayjor = m.Mayjor,
            //    level = m.level,
            //    Comment = m.Comment,
            //    RealName = m.RealName
            //});
            //}).Where(m => UserManager.GetRoles(m.Id).First().Equals(UsersModel.userType))
            //.OrderBy(m => m.Id).Skip(0).Take(2).ToList();
            //.Where(s=> UserManager.GetRoles(s.Id).FirstOrDefault().Equals(UsersModel.userType));
            //.Where(s => s.UserName == "1920135159" && s.Photo.Contains("1920135159"))
            // Console.WriteLine(temp);
            Console.WriteLine("fsf");
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
