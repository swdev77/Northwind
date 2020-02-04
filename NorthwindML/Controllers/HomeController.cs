using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NorthwindML.Models;
using Packt.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.Data;
using Microsoft.ML.Trainers;

namespace NorthwindML.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Northwind db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly static string datasetName = "dataset.txt";
        private readonly static string[] countries = 
            new[] { "Germany", "UK", "USA" };

        public HomeController(
            ILogger<HomeController> logger,  
            Northwind db, 
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }

        public string GetDataPath(string file)
        {
            return Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot", "Data", file);
        }

        private HomeIndexViewModel CreateHomeIndexViewModel()
        {
            return new HomeIndexViewModel
            {
                Categories = db.Categories
                    .Include(c => c.Products),

                GermanyDatasetExists = System.IO.File.Exists(
                    GetDataPath("germany-dataset.txt")
                ),

                UKDatasetExists = System.IO.File.Exists(
                    GetDataPath("uk-dataset.txt")
                ),

                USADatasetExists = System.IO.File.Exists(
                    GetDataPath("usa-dataset.txt")
                )
            };
        }

        public IActionResult Index()
        {
            var model = CreateHomeIndexViewModel();
            return View(model);
        }

        public IActionResult GenerateDatasets()
        {
            foreach(string country in countries)
            {
                IEnumerable<Order> ordersInCountry = db.Orders
                    .Where(o => o.Customer.Country == country)
                    .Include(o => o.OrderDetails)
                    .AsEnumerable();

                IEnumerable<ProductCobought> coboughtProducts =
                    ordersInCountry.SelectMany(o => 
                        from lineItem1 in o.OrderDetails
                        from lineItem2 in o.OrderDetails
                        select new ProductCobought{
                            ProductID = (unit)lineItem1.ProductID,
                            CoboughtProductID = (uint)lineItem2.ProductID
                        })
                        .Where(p => p.ProductID != p.CoboughtProductID)
                        .GroupBy(p => new {p.ProductID, p.CoboughtProductID })
                        .Select(p => p.FirstOrDefault ())
                        .OrderBy(p => p.ProductID)
                        .ThenBy(p => p.CoboughtProductID);
                
                StreamWriter datasetFile = System.IO.File.CreateText(
                    path: GetDataPath($"{country.ToLower()}-{datasetName}")
                );

                datasetFile.WriteLine("ProductID\tCoboughtProductID");
                foreach(var item in coboughtProducts)
                {
                    datasetFile.WriteLine("{0}\t{1}", item.ProductID, item.CoboughtProductID);
                }
                datasetFile.Close();
            }
            var model = CreateHomeIndexViewModel();
            return View("Index", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
