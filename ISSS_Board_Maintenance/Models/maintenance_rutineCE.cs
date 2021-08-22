using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISSS_Board_Maintenance.Models
{
    public class maintenance_rutineCE : maintenance_rutine
    {
        public string employee_user_full_name { get; set; }
        public bool hasFiles { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public maintenance_rutineCE()
        {

        }
        public maintenance_rutineCE(maintenance_rutine baseRutine)
        {
            mr_id = baseRutine.mr_id;
            ms_id = baseRutine.ms_id;
            date = baseRutine.date;
            slots_quantity = baseRutine.slots_quantity;
            slots_available = baseRutine.slots_available;
            transf_capacity = baseRutine.transf_capacity;
            general_protection = baseRutine.general_protection;
            initial_termography = baseRutine.initial_termography;
            general_cleaning = baseRutine.general_cleaning;
            cable_managment = baseRutine.cable_managment;
            retighten = baseRutine.retighten;
            rotulation = baseRutine.rotulation;
            final_termography = baseRutine.final_termography;
            cover_placement = baseRutine.cover_placement;
            protection_capacity = baseRutine.protection_capacity;
            voltage_01 = baseRutine.voltage_01;
            voltage_02 = baseRutine.voltage_02;
            voltage_03 = baseRutine.voltage_03;
            current_01 = baseRutine.current_01;
            current_02 = baseRutine.current_02;
            current_03 = baseRutine.current_03;
            current_n = baseRutine.current_n;
            observations = baseRutine.observations;
            employee_id = baseRutine.employee_id;
            employee_user = baseRutine.employee_user;
            employee_user_full_name = baseRutine.employee_user.GetFullName();
        }
    }
}