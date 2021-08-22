using ISSS_Board_Maintenance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ISSS_Board_Maintenance.Controllers
{
    public class DependencyAPIController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<dependencyCE> Get()
        {
            try
            {
                using (var db = new BM_010_ISSSEntities())
                {
                    List<dependency> dependencies = db.dependencies.ToList();
                    List<dependencyCE> dependencyCE = new List<dependencyCE>();
                    foreach (dependency item in dependencies)
                    {
                        dependencyCE itemCE = new dependencyCE();
                        itemCE.dependency_id = item.dependency_id;
                        itemCE.dependency_name = item.dependency1;
                        dependencyCE.Add(itemCE);
                    }
                    return dependencyCE;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
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
    public class dependencyCE
    {
        public int dependency_id { get; set; }
        public string dependency_name { get; set; }
    }
}