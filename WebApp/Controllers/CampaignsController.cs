using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class CampaignsController : Controller
    {
        // GET: Campaigns
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Campaign()
        {
            return View();
        }
    }
}