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
            if (user_session.session != null)
                return verifySession();
            else
                return View();
        }
        public ActionResult List()
        {
            if (user_session.session != null)
                try
                {
                    using (var db = new BM_010_ISSSEntities())
                    {
                        return View(db.employee_user.Where(u => u.role.Trim() != "admin").ToList());
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
            if (user_session.session != null)
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
                        .Where(u => u.username == user.username && u.password == user.password)
                        .FirstOrDefault();
                    if (userF != default)
                    {
                        if((bool)userF.verification)
                        {
                            user_session.session = userF;
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
            if (user_session.session != null)
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
                if(user1 != default)
                {
                    TempData["userFound"] = "Nombre de usuario ya existente";
                }
                else
                {
                    user.signature = ConvertToBytes(file);
                    if (user.signature.Length > 2097152)
                        TempData["maxSizeReached"] = "Tamaño máximo";
                    else
                    {
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
        public ActionResult HomeAdmin()
        {
            if (user_session.session != null)
                if (user_session.session.role.Trim() == "employee")
                    return View("Home", "User");
                else
                    return View();
            else
                return RedirectToAction("Index", "User");
        }
        public ActionResult Home()
        {
            if (user_session.session != null)
                if (user_session.session.role.Trim() == "admin")
                    return View("HomeAdmin", "User");
                else
                    return View();
            else
                return RedirectToAction("Index", "User");
        }
        private ActionResult verifySession()
        {
            if (user_session.session != null)
                if (user_session.session.role.Trim() == "admin")
                    return RedirectToAction("HomeAdmin", "User");
                else
                    return RedirectToAction("Home", "User");
            else
                return RedirectToAction("Index", "User");
        }
        public ActionResult logOut()
        {
            user_session.session = null;
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