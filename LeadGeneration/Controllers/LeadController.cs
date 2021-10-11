using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeadGeneration.Controllers
{
    public class LeadController : Controller
    {
        // GET: Lead
        public ActionResult Index()
        {
            return View();
        }
    }
}