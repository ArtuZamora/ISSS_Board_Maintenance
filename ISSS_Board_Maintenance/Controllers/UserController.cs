using ISSS_Board_Maintenance.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            if (Session["role"] != null)
                return verifySession();
            else
                return View();
        }
        public ActionResult List()
        {
            if (Session["role"] != null)
                try
                {
                    using (var db = new BM_010_ISSSEntities())
                    {
                        List<employee_user> users = db.employee_user.Where(u => u.role.Trim() != "admin").ToList();
                        List<employee_userCE> usersCE = new List<employee_userCE>();
                        foreach (employee_user user in users)
                        {
                            employee_userCE userCE = new employee_userCE(user);
                            userCE.dependecy_name = user.dependency.dependency1;
                            usersCE.Add(userCE);
                        }
                        return View(usersCE);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            else
                return RedirectToAction("Index", "User");
        }
        public ActionResult Login()
        {
            if (Session["role"] != null)
                return verifySession();
            else
                return View();
        }
        [HttpPost]
        public ActionResult Login(employee_user user)
        {
            if (!string.IsNullOrEmpty(user.username))
            {
                using (var db = new BM_010_ISSSEntities())
                {
                    employee_user userF =
                        db.employee_user
                        .Where(u => u.username == user.username)
                        .FirstOrDefault();
                    if (userF != default)
                        if (userF.password != user.password)
                            userF = default;
                    if (userF != default)
                    {
                        if ((bool)userF.verification)
                        {
                            Session["id"] = userF.employee_id;
                            Session["role"] = userF.role;
                            Session["fullname"] = userF.GetFullName();
                            Session["signature"] = userF.signature;
                            Session["dependency_id"] = userF.dependency_id;
                            Session["dependency_name"] = userF.dependency.dependency1;
                            return verifySession();
                        }
                        else
                        {
                            TempData["unverifiedAcc"] = "Cuenta no verificada";
                            return RedirectToAction("Index", "User");
                        }
                    }
                    else
                    {
                        TempData["alertMessage"] = "Usuario no encontrado.\n Nombre de usuario o contraseña incorrectos.";
                        return RedirectToAction("Index", "User");
                    }
                }
            }
            else
                return RedirectToAction("Index", "User");
        }
        public ActionResult SignIn()
        {
            if (Session["role"] != null)
                return verifySession();
            else
                return View();
        }
        [HttpPost]
        public ActionResult SignIn(employee_user user)
        {
            HttpPostedFileBase file = Request.Files["signatureData"];
            if (string.IsNullOrWhiteSpace(user.name) || string.IsNullOrWhiteSpace(user.last_name)
                || string.IsNullOrWhiteSpace(user.username) || string.IsNullOrWhiteSpace(user.password)
                || file.ContentLength == 0)
                TempData["noFullData"] = "Faltan datos";
            else
            {
                employee_user user1 = null;
                using (var db = new BM_010_ISSSEntities())
                    user1 = db.employee_user.Where(u => u.username == user.username).FirstOrDefault();
                if (user1 != default)
                {
                    TempData["userFound"] = "Nombre de usuario ya existente";
                }
                else
                {
                    if (ConvertToBytes(file).Length > 2097152)
                        TempData["maxSizeReached"] = "Tamaño máximo";
                    else
                    {
                        string path = Server.MapPath("~/Signatures/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        file.SaveAs(path + Path.GetFileName(file.FileName));
                        user.signature = path + Path.GetFileName(file.FileName);
                        user.role = "employee";
                        user.verification = false;
                        using (var db = new BM_010_ISSSEntities())
                        {
                            db.employee_user.Add(user);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("Index", "User");
        }
        public ActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(employee_userCE user)
        {
            HttpPostedFileBase file = Request.Files["signatureData"];
            if (string.IsNullOrWhiteSpace(user.name) || string.IsNullOrWhiteSpace(user.last_name)
                            || string.IsNullOrWhiteSpace(user.username))
                TempData["noFullDataEdit"] = "Faltan datos";
            else
            {
                if (ConvertToBytes(file).Length > 2097152)
                    TempData["maxSizeReachedEdit"] = "Tamaño máximo";
                else
                {
                    if(file.ContentLength != 0)
                    {
                        string path = Server.MapPath("~/Signatures/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        file.SaveAs(path + Path.GetFileName(file.FileName));
                        user.signature = path + Path.GetFileName(file.FileName);
                    }
                    using (var db = new BM_010_ISSSEntities())
                    {
                        employee_user eUser = db.employee_user.Find(user.employee_id);
                        eUser.name = user.name;
                        eUser.last_name = user.last_name;
                        eUser.password = string.IsNullOrWhiteSpace(user.password) ? eUser.password : user.password;
                        eUser.dependency_id = user.dependency_id;
                        eUser.signature = file.ContentLength == 0 ? eUser.signature : user.signature;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index", "User");
        }
        public ActionResult HomeAdmin()
        {
            if (Session["role"] != null)
                if (Session["role"].ToString().Trim() == "employee")
                    return View("Home", "User");
                else
                    return View();
            else
                return RedirectToAction("Index", "User");
        }
        public ActionResult Home()
        {
            if (Session["role"] != null)
                if (Session["role"].ToString().Trim() == "admin")
                    return View("HomeAdmin", "User");
                else
                    return View();
            else
                return RedirectToAction("Index", "User");
        }
        private ActionResult verifySession()
        {
            if (Session["role"] != null)
                if (Session["role"].ToString().Trim() == "admin")
                    return RedirectToAction("HomeAdmin", "User");
                else
                    return RedirectToAction("Home", "User");
            else
                return RedirectToAction("Index", "User");
        }
        public ActionResult logOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "User");
        }
        public ActionResult SuscribeUser(int id)
        {
            using (var db = new BM_010_ISSSEntities())
            {
                employee_user user = db.employee_user.Find(id);
                user.verification = true;
                db.SaveChanges();
                TempData["UsersChanged"] = true;
                return RedirectToAction("Index", "User");
            }
        }
        public ActionResult UnsuscribeUser(int id)
        {
            using (var db = new BM_010_ISSSEntities())
            {
                employee_user user = db.employee_user.Find(id);
                user.verification = false;
                db.SaveChanges();
                TempData["UsersChanged"] = true;
                return RedirectToAction("Index", "User");
            }
        }
        public FileResult GetSignature(int id)
        {
            using (var db = new BM_010_ISSSEntities())
            {
                employee_user user = db.employee_user.Find(id);
                if (string.IsNullOrEmpty(user.signature))
                    return File(new byte[0], "image/png");
                return File(user.signature, "image/png");
            }
        }
        private byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            byte[] fileBytes = null;
            BinaryReader reader = new BinaryReader(file.InputStream);
            fileBytes = reader.ReadBytes((int)file.ContentLength);
            return fileBytes;
        }
    }
}