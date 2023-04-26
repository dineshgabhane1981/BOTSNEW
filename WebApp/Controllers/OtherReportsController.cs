using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using WebApp.ViewModel;
using BOTS_BL;

namespace WebApp.Controllers
{
    public class OtherReportsController : Controller
    {
        OtherReportsRepository ORR = new OtherReportsRepository();
        Exceptions newexception = new Exceptions();
        // GET: OtherReports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Productwise()
        {
            OtherReportProductwiseViewModel objData = new OtherReportProductwiseViewModel();
            List<SellingProductValue> lstTop5SessingProductValue = new List<SellingProductValue>();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                objData.lstTop5Value = ORR.GetTop5SellingProductValue(userDetails.GroupId, userDetails.connectionString);
                objData.lstBottom5Value = ORR.GetBottom5SellingProductValue(userDetails.GroupId, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Productwise");
            }

            return View(objData);
        }

        public ActionResult Manufacturer()
        {
            return View();
        }
        public ActionResult ReportsDownload()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"]; 
            var lstReportDownload = ORR.GetReportDownloadData(userDetails.GroupId);
                    
            return View(lstReportDownload);
        }

        public ActionResult FranchiseeEnquiryReport()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<tblFranchiseeEnquiry> objData = new List<tblFranchiseeEnquiry>();
            try
            {
                objData = ORR.GetFranchiseeEnquiryList(userDetails.GroupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "FranchiseeEnquiryReport");
            }
            return View(objData);
        }
    }
}