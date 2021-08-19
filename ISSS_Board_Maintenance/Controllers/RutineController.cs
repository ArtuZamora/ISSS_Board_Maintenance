using Aspose.Zip;
using Aspose.Zip.Saving;
using ISSS_Board_Maintenance.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ISSS_Board_Maintenance.Controllers
{
    public class RutineController : Controller
    {
        // GET: Rutine
        public ActionResult Index()
        {
            if (Session["role"] != null)
                try
                {
                    using (var db = new BM_010_ISSSEntities())
                    {
                        List<maintenance_rutineCE> rutinesCE = new List<maintenance_rutineCE>();
                        List<maintenance_rutine> rutines = db.maintenance_rutine.ToList();
                        foreach (maintenance_rutine rutine in rutines)
                        {
                            maintenance_rutineCE rutineCE = new maintenance_rutineCE();
                            rutineCE.ms_id = rutine.ms_id;
                            rutineCE.mr_id = rutine.mr_id;
                            rutineCE.date = rutine.date;
                            rutineCE.employee_user_full_name = rutine.employee_user.name + " " + rutine.employee_user.last_name;
                            rutineCE.hasFiles = db.termography_files.Where(tf => tf.mr_id == rutine.mr_id).ToList().Count == 0 ? false : true;
                            rutinesCE.Add(rutineCE);
                        }
                        return View(rutinesCE);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            else
                return RedirectToAction("Index", "User");
        }
        public ActionResult IndexEmployee(string id)
        {
            if (Session["role"] != null)
                try
                {
                    using (var db = new BM_010_ISSSEntities())
                    {
                        List<maintenance_rutineCE> rutinesCE = new List<maintenance_rutineCE>();
                        List<maintenance_rutine> rutines = db.maintenance_rutine.Where(mr => mr.ms_id == id).ToList();
                        foreach (maintenance_rutine rutine in rutines)
                        {
                            maintenance_rutineCE rutineCE = new maintenance_rutineCE();
                            rutineCE.ms_id = rutine.ms_id;
                            rutineCE.mr_id = rutine.mr_id;
                            rutineCE.date = rutine.date;
                            rutineCE.employee_user_full_name = rutine.employee_user.name + " " + rutine.employee_user.last_name;
                            rutineCE.hasFiles = db.termography_files.Where(tf => tf.mr_id == rutine.mr_id).ToList().Count == 0 ? false : true;
                            rutinesCE.Add(rutineCE);
                        }
                        return View(rutinesCE);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            else
                return RedirectToAction("Index", "User");
        }

        // GET: Rutine/Details/5
        public ActionResult Details(int id)
        {
            using (var db = new BM_010_ISSSEntities())
            {
                return View(new maintenance_rutineCE(db.maintenance_rutine.Find(id)));
            }
        }

        public ActionResult DetailsAsPDF(int id)
        {
            return new ActionAsPdf("Details", new { id = id })
            { FileName = id + "R" + DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yy") + ".pdf" };
        }

        // GET: Rutine/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rutine/Create
        [HttpPost]
        public ActionResult Create(maintenance_rutine rutine)
        {
            using (var db = new BM_010_ISSSEntities())
            {
                maintenance_schedule ms = db.maintenance_schedule.Find(rutine.ms_id);
                rutine.description = ms.type;
                rutine.location = ms.location;
                rutine.employee_id = Convert.ToInt32(Session["id"]);
                rutine.date = rutine.date == null ? DateTime.Now : rutine.date;
                DateTime dt1 = ((DateTime)rutine.date).AddHours(DateTime.Now.Hour);
                DateTime dt2 = dt1.AddMinutes(DateTime.Now.Minute);
                DateTime dt3 = dt2.AddSeconds(DateTime.Now.Second);
                rutine.date = dt3;
                string tc = rutine.transf_capacity;
                rutine.transf_capacity = string.IsNullOrEmpty(tc) ? "NO APLICA" : tc;
                string pc = rutine.protection_capacity;
                rutine.protection_capacity = string.IsNullOrEmpty(pc) ? "NO APLICA" : pc;
                rutine.general_protection = GetYesNo(rutine.general_protection);
                rutine.initial_termography = GetYesNo(rutine.initial_termography);
                rutine.general_cleaning = GetYesNo(rutine.general_cleaning);
                rutine.cable_managment = GetYesNo(rutine.cable_managment);
                rutine.retighten = GetYesNo(rutine.retighten);
                rutine.rotulation = GetYesNo(rutine.rotulation);
                rutine.final_termography = GetYesNo(rutine.final_termography);
                rutine.cover_placement = GetYesNo(rutine.cover_placement);
                db.maintenance_rutine.Add(rutine);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "User");
        }

        // GET: Rutine/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rutine/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            using (var db = new BM_010_ISSSEntities())
            {
                db.maintenance_rutine.Remove(db.maintenance_rutine.Find(id));
                db.SaveChanges();
                return RedirectToAction("Index", "User");
            }
        }

        public ActionResult DownloadAttachment(int id)
        {
            using (var db = new BM_010_ISSSEntities())
            {
                List<termography_files> files = db.termography_files.Where(f => f.mr_id == id).ToList();
                using (var ms = new MemoryStream())
                {
                    using (var archive =
                        new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach (termography_files item in files)
                        {
                            var filename = System.IO.Path.GetFileName(item.termography_file);
                            var fileS = System.IO.File.OpenRead(item.termography_file);

                            var fileB = new byte[fileS.Length];
                            fileS.Read(fileB, 0, fileB.Length);

                            var zipEntry = archive.CreateEntry(filename, CompressionLevel.Fastest);
                            using (var zipStream = zipEntry.Open())
                            {
                                zipStream.Write(fileB, 0, fileB.Length);
                            }
                        }
                    }
                    return File(ms.ToArray(),
                        "application/zip",
                        id + "TF" + DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yy") + ".zip");
                }
            }
        }
        public ActionResult UploadTermographyFiles()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadTermographyFiles(int id)
        {
            IList<HttpPostedFileBase> files = Request.Files.GetMultiple("termographyFiles");
            if (files.Count > 0)
                using (var db = new BM_010_ISSSEntities())
                {
                    string path = Server.MapPath("~/TF/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Server.MapPath("~/TF/" + id + "/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    foreach (var file in files)
                    {
                        file.SaveAs(path + Path.GetFileName(file.FileName));
                        termography_files tf = new termography_files();
                        tf.mr_id = id;
                        tf.termography_file = path + Path.GetFileName(file.FileName);
                        db.termography_files.Add(tf);
                    }
                    db.SaveChanges();
                }
            return RedirectToAction("Index", "User");
        }
        private string GetYesNo(string verif)
        {
            return string.IsNullOrEmpty(verif) ? "NO" : "SI";
        }
    }
}
