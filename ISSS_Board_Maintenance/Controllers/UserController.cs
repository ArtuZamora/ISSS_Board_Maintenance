using ISSS_Board_Maintenance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISSS_Board_Maintenance.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(employee_user user)
        {
            if (string.IsNullOrEmpty(user.name) || string.IsNullOrEmpty(user.password))
            {
                if(user.role.Trim() == "admin")
                    return RedirectToAction("HomeAdmin", "User");
                else
                    return RedirectToAction("Home", "User");
            }
            else
                return RedirectToAction("Index", "User");
        }
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(employee_user user)
        {
            return View();
        }
        public ActionResult HomeAdmin()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        private void verifySession()
        {
            if (string.IsNullOrEmpty(employee_user.name) || string.IsNullOrEmpty(user.password))
            {
                if (user.role.Trim() == "admin")
                    return RedirectToAction("HomeAdmin", "User");
                else
                    return RedirectToAction("Home", "User");
            }
            else
                return RedirectToAction("Index", "User");
        }
    }
}