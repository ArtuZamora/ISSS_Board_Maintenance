using ISSS_Board_Maintenance.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISSS_Board_Maintenance.Controllers
{
    public class ScheduleController : Controller
    {
        // GET: Schedule
        public ActionResult Index()
        {
            if (Session["role"] != null)
                try
                {
                    using (var db = new BM_010_ISSSEntities())
                    {
                        var list = db.maintenance_schedule.ToList();
                        List<maintenance_scheduleCE> collection = new List<maintenance_scheduleCE>();
                        foreach (var item in list)
                        {
                            maintenance_scheduleCE obj = new maintenance_scheduleCE();
                            obj.ms_id = item.ms_id;
                            obj.type = item.type;
                            obj.dependency = item.dependency;
                            obj.building = item.building;
                            obj.location = item.location;
                            obj.nomenclature = item.nomenclature;
                            obj.month_scheduled = item.month_scheduled;
                            obj.level = item.level;
                            obj.maintenance_rutine = item.maintenance_rutine;
                            obj.month_scheduled_name = new DateTime(2000, item.month_scheduled, 1)
                                .ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                            collection.Add(obj);
                        }
                        return View(collection);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            else
                return RedirectToAction("Index", "User");
        }

        public ActionResult IndexEmployee(int id)
        {
            if (Session["role"] != null)
                try
                {
                    using (var db = new BM_010_ISSSEntities())
                    {
                        var list = db.maintenance_schedule
                            .ToList()
                            .Where(ms => ms.month_scheduled == id 
                            && ms.dependency_id == Convert.ToInt32(Session["dependency_id"]));
                        List<maintenance_scheduleCE> collection = new List<maintenance_scheduleCE>();
                        foreach (var item in list)
                        {
                            maintenance_scheduleCE obj = new maintenance_scheduleCE();
                            obj.ms_id = item.ms_id;
                            obj.type = item.type;
                            obj.dependency = item.dependency;
                            obj.building = item.building;
                            obj.location = item.location;
                            obj.nomenclature = item.nomenclature;
                            obj.month_scheduled = item.month_scheduled;
                            obj.level = item.level;
                            obj.maintenance_rutine = item.maintenance_rutine;
                            obj.month_scheduled_name = new DateTime(2000, item.month_scheduled, 1)
                                .ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                            collection.Add(obj);
                        }
                        return View(collection);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            else
                return RedirectToAction("Index", "User");
        }

        // GET: Schedule/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Schedule/Create
        [HttpPost]
        public ActionResult Create(maintenance_schedule schedule)
        {
            if (string.IsNullOrWhiteSpace(schedule.building) || string.IsNullOrWhiteSpace(schedule.level)
                || string.IsNullOrWhiteSpace(schedule.location) || string.IsNullOrWhiteSpace(schedule.nomenclature)
                || string.IsNullOrWhiteSpace(schedule.type) || string.IsNullOrWhiteSpace(schedule.ms_id))
                TempData["noFullDataSchedule"] = "noFullDataSchedule";
            else
            {
                using (var db = new BM_010_ISSSEntities())
                {
                    if (db.maintenance_schedule.Find(schedule.ms_id) == null)
                    {
                        db.maintenance_schedule.Add(schedule);
                        db.SaveChanges();
                    }
                    else
                        TempData["noValidCode"] = "Codigo existente";
                }
            }
            return RedirectToAction("Index", "User");
        }

        // GET: Schedule/Edit/5
        public ActionResult Edit()
        {
            return View();
        }

        // POST: Schedule/Edit/5
        [HttpPost]
        public ActionResult Edit(maintenance_schedule schedule)
        {
            if (string.IsNullOrWhiteSpace(schedule.building) || string.IsNullOrWhiteSpace(schedule.level)
                || string.IsNullOrWhiteSpace(schedule.location) || string.IsNullOrWhiteSpace(schedule.nomenclature)
                || string.IsNullOrWhiteSpace(schedule.type))
                TempData["noFullDataScheduleEdit"] = schedule.ms_id;
            else
            {
                using (var db = new BM_010_ISSSEntities())
                {
                    maintenance_schedule sc = db.maintenance_schedule.Find(schedule.ms_id);
                    sc.type = schedule.type;
                    sc.dependency_id = schedule.dependency_id;
                    sc.building = schedule.building;
                    sc.level = schedule.level;
                    sc.location = schedule.location;
                    sc.nomenclature = schedule.nomenclature;
                    sc.month_scheduled = schedule.month_scheduled;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "User");
        }

        // GET: Schedule/Delete/5
        public ActionResult Delete(string id)
        {
            using (var db = new BM_010_ISSSEntities())
            {
                db.maintenance_schedule.Remove(db.maintenance_schedule.Find(id));
                db.SaveChanges();
                return RedirectToAction("Index", "User");
            }
        }
    }
}
