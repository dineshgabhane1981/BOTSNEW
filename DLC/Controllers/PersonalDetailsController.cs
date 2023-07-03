using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using DLC.App_Start;
using DLC.ViewModel;
using BOTS_BL.Models;

namespace DLC.Controllers
{
    public class PersonalDetailsController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        // GET: PersonalDetails
        public ActionResult Index()
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            var lstData = DCR.GetPublishDLCProfileConfig(sessionVariables.GroupId);
            return View(lstData);
        }
    }
}