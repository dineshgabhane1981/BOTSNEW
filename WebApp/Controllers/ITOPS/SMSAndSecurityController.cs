using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.ITOPS
{
    public class SMSAndSecurityController : Controller
    {
        // GET: SMSAndSecurity
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SecurityKey()
        {
            return View();
        }
    }
}