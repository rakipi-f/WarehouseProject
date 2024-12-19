using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using RestSharp;
using WarehouseAsp.Models;

namespace WarehouseAsp.Controllers
{
    [Authorize]
    [RoutePrefix("Movements")]
    public class MovementsController : Controller
    {
        // get methods
        public async Task<ActionResult> List(int page = 1, int pageSize = 10, int pages = 2, string search = "", string previousSearch = "")
        {

            // quando applico una NUOVA ricerca devo tornare alla pag. 1
            if ((search != "" && !string.Equals(search, previousSearch, StringComparison.OrdinalIgnoreCase)) || pages == 1)
                page = 1;

            string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
            var client = new RestClient($"{BaseUrl}/Movements");
            var request = new RestRequest();
            request.AddQueryParameter("page", page.ToString());
            request.AddQueryParameter("pagesize", pageSize.ToString());
            request.AddQueryParameter("search", search.ToString());
            var response = await client.GetAsync<PaginetedResult<WarehouseMovement>>(request);
            ViewBag.PreviousSearch = search;
            return View(response);
        }

        // Create methods
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WarehouseMovement entity)
        {
            string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
            var client = new RestClient($"{BaseUrl}/Movements");
            var request = new RestRequest();
            request.AddJsonBody(entity);
            var response = await client.PostAsync<WarehouseMovement>(request);
            return RedirectToAction("List");

        }

        // Edit methods
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
            var client = new RestClient($"{BaseUrl}/Movements/{id}");
            var request = new RestRequest();
            var response = await client.GetAsync<WarehouseMovement>(request);
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WarehouseMovement entity)
        {
            string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
            var client = new RestClient($"{BaseUrl}/Movements/{entity.Id}");
            var request = new RestRequest();
            request.AddJsonBody(entity);
            var response = await client.PutAsync<WarehouseMovement>(request);
            return RedirectToAction("List");
        }

        //Delete methods
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
            var client = new RestClient($"{BaseUrl}/Movements/{id}");
            var request = new RestRequest();
            var response = await client.GetAsync<WarehouseMovement>(request);
            return View(response);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
            var client = new RestClient($"{BaseUrl}/Movements/{id}");
            var request = new RestRequest();
            var response = await client.DeleteAsync<WarehouseMovement>(request);
            return RedirectToAction("List");
        }


    }
}
