using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.ITOPS
{
    public class AddMemberController : Controller
    {
        // GET: AddMember
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BulkImport()
        {
            return View();
        }
    }
}