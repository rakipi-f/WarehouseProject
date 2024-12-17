﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using RestSharp;
using WarehouseAsp.Models;

namespace WarehouseAsp.Controllers
{
    [RoutePrefix("Movements")]
    public class MovementsController : Controller
    {
        // GET: Movements
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

    }
}