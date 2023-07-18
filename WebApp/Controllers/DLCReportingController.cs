using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.ComponentModel;
using BOTS_BL;
using System.Globalization;
using System.Web.Script.Serialization;
using BOTS_BL.Models.Reports;
using System.Net.Mail;
using System.Text;
using WebApp.ViewModel;
using System.Reflection;

namespace WebApp.Controllers
{
    public class DLCReportingController : Controller
    {
        DLCReportingRepository DLCR = new DLCReportingRepository();
        
        Exceptions newexception = new Exceptions();
        // GET: DLCReporting
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var data = DLCR.GetDLCReportings(userDetails.GroupId, "0", "0", "4");
            return View(data);
        }

        [HttpPost]
        public JsonResult GetCampaignSummary(string flag, string year, string month)
        {
            List<DLCReporting> data = new List<DLCReporting>();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                data = DLCR.GetDLCReportings(userDetails.GroupId, month, year, flag);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignSummary");
            }
            return new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult DLCNewReg()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var data = DLCR.GetDLCNew(userDetails.GroupId);
            return View(data);
        }

        public ActionResult DLCDetail(string SourceId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var data = DLCR.GetDLCDataDetail(userDetails.GroupId,SourceId);
            return PartialView("_DLCNewDetail", data);
        }
    }
}