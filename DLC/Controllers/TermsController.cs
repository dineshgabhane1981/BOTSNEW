using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using DLC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DLC.Controllers
{
    public class TermsController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        // GET: Terms
        public ActionResult Index()
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            var lstData = DCR.GetTNC(sessionVariables.GroupId);
            return View(lstData);
        }
    }
}