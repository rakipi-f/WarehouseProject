using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WarehouseAsp.Models;

namespace WarehouseAsp.Controllers
{
    [Authorize]
    [RoutePrefix("Statistics")]
    public class StatisticsController : Controller
    {
        // GET: Statics
        public async Task<ActionResult> List(int id=1)
        {
            string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
            var client = new RestClient($"{BaseUrl}/WarehouseMovements/{id}");
            var request = new RestRequest();
            var response = await client.GetAsync<ViewModelConsole>(request);


            if (response?.Movements == null)
            {
                ViewBag.ErrorMessage = "Id prodotto non esistente";
                return View();
            }

            
            var movementsGroupedByMonth = response.Movements
                .GroupBy(m => new { m.Date.Year, m.Date.Month })
                .Select(g => new ChartView
                {
                    //currentculture per i mesi in italiano
                    Month = $"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(g.Key.Month)} {g.Key.Year}",
                    Movements = g.OrderBy(m => m.Date).ToList(), // Ordina i movimenti per data
                    Id= id
                })
                .OrderBy(m => DateTime.ParseExact(m.Month, "MMMM yyyy", CultureInfo.InvariantCulture))
                .ToList();


            double sum = 0;
            foreach (var item in movementsGroupedByMonth)
            {
                foreach (var movement in item.Movements)
                {
                    sum += movement.Qty;
                }
                item.GiacenzaTot = sum;
            }

            return View(movementsGroupedByMonth);
        }

        public ActionResult Search(int id)
        {

            return RedirectToAction("List", new { id });
        }
    }
}

