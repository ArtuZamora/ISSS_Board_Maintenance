using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISSS_Board_Maintenance.Models
{
    public class maintenance_scheduleCE : maintenance_schedule
    {
        public string month_scheduled_name { get; set; }
    }
}