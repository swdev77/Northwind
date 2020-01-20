using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Packt.Shared;

namespace PacktFeatures.Pages
{
    public class EmployeeOrdersPageModel : PageModel
    {
        private readonly Northwind db;

        public EmployeeOrdersPageModel(Northwind injectedContext)
        {
            this.db = injectedContext;
        }    

        public Employee Employee { get; set; }
        public string EmployeeName { get; set; }
        public IEnumerable<Order> Orders { get; set; }

        public void OnGet(int EmployeeID)
        {
            Employee = db.Employees
                .FirstOrDefault(e => e.EmployeeID == EmployeeID);  
            
            EmployeeName = string.Join(" ", Employee?.LastName, Employee?.FirstName);

            Orders = db.Orders
                .Where(o => o.EmployeeID == EmployeeID)
                .ToArray();
        }
    }
}