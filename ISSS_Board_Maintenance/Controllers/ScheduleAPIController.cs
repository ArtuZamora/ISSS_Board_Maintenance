using ISSS_Board_Maintenance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ISSS_Board_Maintenance.Controllers
{
    public class ScheduleAPIController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(string id)
        {
            try
            {
                using (var db = new BM_010_ISSSEntities())
                {
                    maintenance_schedule schedule = db.maintenance_schedule.Find(id);
                    if (schedule != default)
                    {
                        schedule schedule1 = new schedule();
                        schedule1.ms_id = schedule.ms_id;
                        schedule1.type = schedule.type;
                        schedule1.dependency_id = schedule.dependency.dependency_id;
                        schedule1.building = schedule.building;
                        schedule1.level = schedule.level;
                        schedule1.location = schedule.location;
                        schedule1.nomenclature = schedule.nomenclature;
                        schedule1.month_scheduled = schedule.month_scheduled;
                        return Ok(schedule1);
                    }
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Get(string id, int id2)
        {
            try
            {
                using (var db = new BM_010_ISSSEntities())
                {
                    List<maintenance_schedule> schedules = db.maintenance_schedule.Where(ms => ms.month_scheduled == id2).ToList();
                    List<string> schedulesCE = new List<string>();
                    foreach (maintenance_schedule item in schedules)
                    {
                        schedulesCE.Add(item.ms_id);
                    }
                    return Ok(schedulesCE.ToArray());
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
    class schedule
    {
        public string ms_id { get; set; }
        public string type { get; set; }
        public int dependency_id { get; set; }
        public string building { get; set; }
        public string level { get; set; }
        public string location { get; set; }
        public string nomenclature { get; set; }
        public int month_scheduled { get; set; }
    }
}