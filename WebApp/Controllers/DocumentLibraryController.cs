using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class DocumentLibraryController : Controller
    {
        // GET: DocumentLibrary
        public ActionResult Index()
        {
            return View();
        }
    }
}