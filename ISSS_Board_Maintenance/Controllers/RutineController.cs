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
            if (user_session.session != null)
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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
                            var zipEntry = archive.CreateEntry(item.termography_file_name + "." + item.termography_file_extension, CompressionLevel.Fastest);
                            using (var zipStream = zipEntry.Open())
                            {
                                zipStream.Write(item.termography_file, 0, item.termography_file.Length);
                            }
                        }
                    }
                    return File(ms.ToArray(),
                        "application/zip",
                        id + "TF" + DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yy") + ".zip");
                }
            }

        }
    }
}
