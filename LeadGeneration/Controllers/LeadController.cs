using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;

namespace LeadGeneration.Controllers
{
    public class LeadController : Controller
    {
        // GET: Lead
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddLead()
        {
            SALES_tblLeads objData = new SALES_tblLeads();
            return View(objData);
        }
    }
}