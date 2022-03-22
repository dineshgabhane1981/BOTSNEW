using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.App_Start;

namespace WebApp.Controllers.ITOPS
{
    public class NameAndMobileController : Controller
    {
        // GET: NameAndMobile
        public ActionResult Index(string groupId)
        {
            CommonFunctions common = new CommonFunctions();
            groupId = common.DecryptString(groupId);
            Session["GroupId"] = groupId;
            return View();
        }

        public ActionResult ChangeMobile()
        {
            var groupId = (string)Session["GroupId"];
            return View();
        }
    }
}