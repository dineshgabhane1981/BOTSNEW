using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Feedback.Controllers
{
    public class AabharBonusController : Controller
    {
        Exceptions newexception = new Exceptions();
        FeedBackRepository FBR = new FeedBackRepository();// GET: Feedback
        // GET: AabharBonus
        public ActionResult Index(string outletid)
        {
            string groupid = string.Empty;
            if (!string.IsNullOrEmpty(outletid))
            {
                groupid = outletid.Substring(0, 4);
                ViewBag.GroupId = groupid;
                ViewBag.OutletId = outletid;
                ViewBag.lstlocation = FBR.GetLocationList(groupid);
            }
            else
            {
                ViewBag.lstlocation = "";
            }
            return View();
        }
    }
}