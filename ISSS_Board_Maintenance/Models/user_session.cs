using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISSS_Board_Maintenance.Models
{
    public static class user_session
    {
        public static int employee_id { get; set; }
        public static string name { get; set; }
        public static string last_name { get; set; }
        public static byte[] signature { get; set; }
        public static string role { get; set; }
        public static bool? verification { get; set; }
        public static string username { get; set; }
        public static string password { get; set; }
    }
}