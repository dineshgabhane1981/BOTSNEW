﻿using BOTS_BL;
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
    public class OptoutController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        Exceptions newexception = new Exceptions();
        // GET: Optout
        public ActionResult Index()
        {            
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            DLCDashboardContent objData = new DLCDashboardContent();
            objData = DCR.GetDLCDashboardContent(sessionVariables.GroupId, sessionVariables.MobileNo);
            objData.MobileNo = sessionVariables.MobileNo;

            return View(objData);
        }
        public ActionResult UpdateOptout(string IsOptout)
        {
            bool status = false;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            try
            {
                status = DCR.UpdateOptout(sessionVariables.GroupId, sessionVariables.MobileNo, Convert.ToBoolean(IsOptout));
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateOptout");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}