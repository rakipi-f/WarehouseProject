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
    
    [RoutePrefix("Products")]
    public class ProductsController : Controller
    {

        // GET: Products
        public async Task<ActionResult> List()
        {
            string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
            var client = new RestClient($"{BaseUrl}/Products");
            var request = new RestRequest();
            var response = await client.GetAsync<List<ProductDto>>(request);
            return View(response);
        }

        public async Task<ActionResult> Details(int id)
        {
            string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
            var client = new RestClient($"{BaseUrl}/Products/{id}");
            var request = new RestRequest();
            var response = await client.GetAsync<Product>(request);

            return View(response);
        }


    }
}