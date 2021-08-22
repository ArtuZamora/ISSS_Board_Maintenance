using ISSS_Board_Maintenance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ISSS_Board_Maintenance.Controllers
{
    public class UserAPIController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public employee_userCE Get(int id)
        {
            try
            {
                using (var db = new BM_010_ISSSEntities())
                {
                    employee_userCE user = new employee_userCE(db.employee_user.Find(id));
                    return user;
                }
            }
            catch
            {
                return null;
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
}