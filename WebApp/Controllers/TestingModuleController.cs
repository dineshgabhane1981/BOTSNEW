using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.App_Start;

namespace WebApp.Controllers
{
    public class TestingModuleController : Controller
    {
        // GET: TestingModule
        public ActionResult Index(string groupId)
        {
            ViewBag.GroupId = groupId;
            return View();
        }
    }
}