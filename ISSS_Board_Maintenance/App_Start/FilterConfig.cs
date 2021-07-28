using System.Web;
using System.Web.Mvc;

namespace ISSS_Board_Maintenance
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
