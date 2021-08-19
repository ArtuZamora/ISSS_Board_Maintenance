using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using ISSS_Board_Maintenance.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISSS_Board_Maintenance.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult GetVoltage(string id)
        {
            string[] categories;
            Data voltage01, voltage02, voltage03;
            using (var db = new BM_010_ISSSEntities())
            {
                var years = db.Database.SqlQuery<DateTime>(@"SELECT DISTINCT CONVERT(DATE, date, 101) date
                    FROM maintenance_rutine 
                    WHERE ms_id = @ms_id", new SqlParameter("@ms_id", id)).ToArray();
                int length = years.Length;
                categories = new string[length];
                for (int i = 0; i < length; i++)
                {
                    categories[i] = years[i].ToString("dd/MM/yyyy");
                }

                var v1 = db.Database.SqlQuery<decimal>(@"SELECT AVG(voltage_01) voltage 
                    FROM maintenance_rutine 
                    WHERE ms_id = @ms_id1
                    GROUP BY CONVERT(DATE, date, 101)", new SqlParameter("@ms_id1", id)).ToArray();
                object[] object_array = new object[v1.Length];
                v1.CopyTo(object_array, 0);
                voltage01 = new Data(object_array);

                var v2 = db.Database.SqlQuery<decimal>(@"SELECT AVG(voltage_02) voltage 
                    FROM maintenance_rutine 
                    WHERE ms_id = @ms_id2
                    GROUP BY CONVERT(DATE, date, 101)", new SqlParameter("@ms_id2", id)).ToArray();
                object[] object_array2 = new object[v2.Length];
                v2.CopyTo(object_array2, 0);
                voltage02 = new Data(object_array2);

                var v3 = db.Database.SqlQuery<decimal>(@"SELECT AVG(voltage_03) voltage 
                    FROM maintenance_rutine 
                    WHERE ms_id = @ms_id3
                    GROUP BY CONVERT(DATE, date, 101)", new SqlParameter("@ms_id3", id)).ToArray();
                object[] object_array3 = new object[v3.Length];
                v3.CopyTo(object_array3, 0);
                voltage03 = new Data(object_array3);
            }

            Highcharts columnChart = new Highcharts("linechartVoltage");
            columnChart.InitChart(new Chart()
            {
                Type = DotNet.Highcharts.Enums.ChartTypes.Line,
                BackgroundColor = new BackColorOrGradient(System.Drawing.Color.AliceBlue),
                Style = "fontWeight: 'bold', fontSize: '17px'",
                BorderColor = System.Drawing.Color.LightBlue,
                BorderRadius = 0,
                BorderWidth = 2

            });

            columnChart.SetTitle(new Title()
            {
                Text = "Gráfico de voltajes"
            });

            columnChart.SetXAxis(new XAxis()
            {
                Type = AxisTypes.Category,
                Title = new XAxisTitle() { Text = "Fechas", Style = "fontWeight: 'bold', fontSize: '17px'" },
                Categories = categories
            });

            columnChart.SetYAxis(new YAxis()
            {
                Title = new YAxisTitle()
                {
                    Text = "Voltaje (V)",
                    Style = "fontWeight: 'bold', fontSize: '17px'"
                },
                ShowFirstLabel = true,
                ShowLastLabel = true
            });

            columnChart.SetLegend(new Legend
            {
                Enabled = true,
                BorderColor = System.Drawing.Color.CornflowerBlue,
                BorderRadius = 6,
                BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFADD8E6"))
            });

            columnChart.SetSeries(new Series[]
            {
                new Series{

                    Name = "Voltaje L1 - L2",
                    Data = voltage01
                },
                new Series()
                {
                    Name = "Voltaje L1 - L3",
                    Data = voltage02
                },
                new Series()
                {
                    Name = "Voltaje L2 - L3",
                    Data = voltage03
                }
            }
            );
            return View(columnChart);
        }
        public ActionResult GetCurrent(string id)
        {
            string[] categories;
            Data current01, current02, current03, currentn;
            using (var db = new BM_010_ISSSEntities())
            {
                var years = db.Database.SqlQuery<DateTime>(@"SELECT DISTINCT CONVERT(DATE, date, 101) date
                    FROM maintenance_rutine 
                    WHERE ms_id = @ms_id", new SqlParameter("@ms_id", id)).ToArray();
                int length = years.Length;
                categories = new string[length];
                for (int i = 0; i < length; i++)
                {
                    categories[i] = years[i].ToString("dd/MM/yyyy");
                }

                var v1 = db.Database.SqlQuery<decimal>(@"SELECT AVG(current_01) [current] 
                    FROM maintenance_rutine 
                    WHERE ms_id = @ms_id1
                    GROUP BY CONVERT(DATE, date, 101)", new SqlParameter("@ms_id1", id)).ToArray();
                object[] object_array = new object[v1.Length];
                v1.CopyTo(object_array, 0);
                current01 = new Data(object_array);

                var v2 = db.Database.SqlQuery<decimal>(@"SELECT AVG(current_02) [current]  
                    FROM maintenance_rutine 
                    WHERE ms_id = @ms_id2
                    GROUP BY CONVERT(DATE, date, 101)", new SqlParameter("@ms_id2", id)).ToArray();
                object[] object_array2 = new object[v2.Length];
                v2.CopyTo(object_array2, 0);
                current02 = new Data(object_array2);

                var v3 = db.Database.SqlQuery<decimal>(@"SELECT AVG(current_03) [current] 
                    FROM maintenance_rutine 
                    WHERE ms_id = @ms_id3
                    GROUP BY CONVERT(DATE, date, 101)", new SqlParameter("@ms_id3", id)).ToArray();
                object[] object_array3 = new object[v3.Length];
                v3.CopyTo(object_array3, 0);
                current03 = new Data(object_array3);

                var v4 = db.Database.SqlQuery<decimal>(@"SELECT AVG(current_n) [current] 
                    FROM maintenance_rutine 
                    WHERE ms_id = @ms_id3
                    GROUP BY CONVERT(DATE, date, 101)", new SqlParameter("@ms_id3", id)).ToArray();
                object[] object_array4 = new object[v4.Length];
                v4.CopyTo(object_array4, 0);
                currentn = new Data(object_array4);
            }

            Highcharts columnChart = new Highcharts("linechartCurrent");
            columnChart.InitChart(new Chart()
            {
                Type = DotNet.Highcharts.Enums.ChartTypes.Line,
                BackgroundColor = new BackColorOrGradient(System.Drawing.Color.AliceBlue),
                Style = "fontWeight: 'bold', fontSize: '17px'",
                BorderColor = System.Drawing.Color.LightBlue,
                BorderRadius = 0,
                BorderWidth = 2

            });

            columnChart.SetTitle(new Title()
            {
                Text = "Gráfico de corrientes"
            });

            columnChart.SetXAxis(new XAxis()
            {
                Type = AxisTypes.Category,
                Title = new XAxisTitle() { Text = "Fechas", Style = "fontWeight: 'bold', fontSize: '17px'" },
                Categories = categories
            });

            columnChart.SetYAxis(new YAxis()
            {
                Title = new YAxisTitle()
                {
                    Text = "Corriente (A)",
                    Style = "fontWeight: 'bold', fontSize: '17px'"
                },
                ShowFirstLabel = true,
                ShowLastLabel = true
            });

            columnChart.SetLegend(new Legend
            {
                Enabled = true,
                BorderColor = System.Drawing.Color.CornflowerBlue,
                BorderRadius = 6,
                BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFADD8E6"))
            });

            columnChart.SetSeries(new Series[]
            {
                new Series{

                    Name = "Corriente L1",
                    Data = current01
                },
                new Series()
                {
                    Name = "Corriente L2",
                    Data = current02
                },
                new Series()
                {
                    Name = "Corriente L3",
                    Data = current03
                },
                new Series()
                {
                    Name = "Corriente N",
                    Data = currentn
                }
            }
            );
            return View(columnChart);
        }
    }
}