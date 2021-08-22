using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISSS_Board_Maintenance.Models
{
    public class employee_userCE : employee_user
    {
        public string dependecy_name { get; set; }
        public employee_userCE()
        {

        }
        public employee_userCE(employee_user base_employee)
        {
            this.employee_id = base_employee.employee_id;
            this.verification = base_employee.verification;
            this.username = base_employee.username;
            this.name = base_employee.name;
            this.last_name = base_employee.last_name;
            this.dependency_id = base_employee.dependency_id;
            this.signature = base_employee.signature;
        }
    }
}