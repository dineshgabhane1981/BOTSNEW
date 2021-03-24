using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chitale.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LeaderBoard()
        {
            return View();
        }
        public ActionResult OrderToInvoice()
        {
            return View();
        }
    }
}