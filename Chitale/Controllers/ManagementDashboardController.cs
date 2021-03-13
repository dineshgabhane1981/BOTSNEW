using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chitale.Controllers
{
    public class ManagementDashboardController : Controller
    {
        // GET: ManagementDashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ParticipantList()
        {
            return View();
        }
        public ActionResult LeaderBoard()
        {
            return View();
        }
        public ActionResult OrdertoRavanaDays()
        {
            return View();
        }
        public ActionResult InvoicetoOrder()
        {
            return View();
        }
       

    }
}