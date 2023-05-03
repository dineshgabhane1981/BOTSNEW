using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class EventsController : Controller
    {
        // GET: Events List
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateEvent()
        {
            return View();
        }
        public ActionResult EventReport()
        {
            return View();
        }
        public ActionResult EventCustomers()
        {
            return View();
        }
    }
}