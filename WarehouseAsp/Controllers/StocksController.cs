using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WarehouseAsp.Dtos;
using WarehouseAsp.Models;

namespace WarehouseAsp.Controllers
{
    [RoutePrefix("Stocks")]
    public class StocksController : Controller
    {
        // GET: Stocks
        public async Task<ActionResult> List(int page = 1, int pageSize = 10, int pages= 2, string search="", string previousSearch="")
        {

            // quando applico una NUOVA ricerca devo tornare alla pag. 1
            if ((search != "" && !string.Equals(search, previousSearch, StringComparison.OrdinalIgnoreCase)) || pages == 1)
                page = 1;
            string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
            var client = new RestClient($"{BaseUrl}/Products");
            var request = new RestRequest();
            request.AddQueryParameter("page", page.ToString());
            request.AddQueryParameter("pagesize", pageSize.ToString());
            request.AddQueryParameter("search", search.ToString());
            var response = await client.GetAsync<PaginetedResult<ProductDto>>(request);
            ViewBag.PreviousSearch = search;

            //ora ho la lista dei prodotti

            PaginetedResult<Stocks> stocks = new PaginetedResult<Stocks>
            {
                Results = new List<Stocks>(),
                page = response.page,
                pageSize = response.pageSize,
                Pages = response.Pages,
                search = response.search
            };
            foreach (var product in response.Results)
            {
                //per ogni prodotto vado a fare la chiamata per ottenerne la giacenza
                string BaseUrl2 = WebConfigurationManager.AppSettings["BaseUrl"];
                var client2 = new RestClient($"{BaseUrl}/WarehouseMovements/{product.Id}");
                var request2 = new RestRequest();
                var response2 = await client2.GetAsync(request2);
                ViewModelConsole movements = JsonConvert.DeserializeObject<ViewModelConsole>(response2.Content);

                stocks.Results.Add(new Stocks
                {
                    Id = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    Stock = movements.Giacenza
                });
            }

            return View(stocks);
        }
    }
}