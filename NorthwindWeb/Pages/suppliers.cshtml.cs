using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Packt.Shared;

namespace NorthwindWeb.Pages
{
    public class SupplierModel : PageModel
    {
        private Northwind db;

        [BindProperty]
        public Supplier Supplier { get; set; }

        public SupplierModel(Northwind injectedContext)
        {
            db = injectedContext;
        }
        public IEnumerable<string> Suppliers { get; set; }

        public void OnGet()
        {
            ViewData["Title"] = "Northwind Web Site - Suppliers.";

            Suppliers = db.Suppliers.Select(s => s.CompanyName);
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid) 
            {
                db.Suppliers.Add(Supplier);
                db.SaveChanges();

                return RedirectToPage("/suppliers");
            }
            return Page();
        }
    }
}