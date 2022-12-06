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
        Exception ex = new Exception();
        // GET: DLCReporting
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var data = DLCR.GetDLCReportings(userDetails.GroupId, "0", "0", "4");
            return View(data);
        }
    }
}