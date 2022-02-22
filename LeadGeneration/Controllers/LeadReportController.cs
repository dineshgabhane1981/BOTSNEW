using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Models.SalesLead;
using BOTS_BL.Repository;
using LeadGeneration.ViewModel;

namespace LeadGeneration.Controllers
{
    public class LeadReportController : Controller
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();
        SalesLeadRepository SLR = new SalesLeadRepository();
        LeadReportsRepository LRR = new LeadReportsRepository();
        // GET: LeadReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MeetingMatrix()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            LeadViewModel objviewmodel = new LeadViewModel();
            objviewmodel.lstSalesManager = SLR.GetSalesManager();
            return View(objviewmodel);
        }

        public ActionResult GetMeetingMatrixList(string searchData)
        {
            List<MeetingMatrix> objMeetingMatrix = new List<MeetingMatrix>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(searchData);
            foreach (Dictionary<string, object> item in objData)
            {
                var isMTD= Convert.ToString(item["isMTD"]);
                var salesManager = Convert.ToString(item["SalesManager"]);
                var frmDate = Convert.ToString(item["frmDate"]);
                var toDate = Convert.ToString(item["toDate"]);
                var MeetingOrCall = Convert.ToString(item["MeetingOrCall"]);
                objMeetingMatrix = LRR.GetMeetingMatrixReport(isMTD,salesManager, frmDate, toDate, MeetingOrCall);
            }

            return PartialView("_MeetingMatrixListing", objMeetingMatrix);
        }

        public ActionResult GetMeetingMatrixDetailReport(string searchData)
        {
            List<SalesLead> objLeads = new List<SalesLead>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(searchData);
            foreach (Dictionary<string, object> item in objData)
            {
                var salesManager = Convert.ToString(item["SalesManager"]);
                var frmDate = Convert.ToString(item["frmDate"]);
                var toDate = Convert.ToString(item["toDate"]);
                var MeetingOrCall = Convert.ToString(item["MeetingOrCall"]);
                var type = Convert.ToString(item["type"]);

                objLeads = LRR.GetDetailMatrixReportData(salesManager, frmDate, toDate, MeetingOrCall, type);
            }

            return PartialView("_MeetingMatrixDetails", objLeads);
        }


        public ActionResult SalesMatrix()
        {
            List<SalesMatrix> lstsalesmatrix = new List<SalesMatrix>();
            List<SelectListItem> MonthList = new List<SelectListItem>();
            int month = DateTime.Now.Month;

            int count = 1;
            for (int i = 1; i <= 12; i++)
            {
                //MonthList.Add(new SelectListItem
                //{
                //    Text = "Select Month",
                //    Value = "0"
                //});
                MonthList.Add(new SelectListItem
                {
                    Text = Convert.ToString(DateTime.Now.AddMonths(i).ToString("MMM")),
                    Value = Convert.ToString(DateTime.Now.AddMonths(i).Month)
                });
                count++;
            }
            List<SelectListItem> YearList = new List<SelectListItem>();
            int year = DateTime.Now.Year;
            YearList.Add(new SelectListItem
            {
                Text = Convert.ToString(DateTime.Now.AddYears(1).Year.ToString()),
                Value = Convert.ToString(year+1)
            });
            for (int i = 0; i <= 10; i++)
            {
               
                YearList.Add(new SelectListItem
                {
                    Text = Convert.ToString(DateTime.Now.AddYears(-i).Year.ToString()),
                    Value = Convert.ToString(year - i)
                });
            }           
           
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            LeadViewModel objviewmodel = new LeadViewModel();
            objviewmodel.lstSalesManager = SLR.GetSalesManager();
            ViewBag.MonthList = MonthList;
            ViewBag.YearList = YearList;
           // lstsalesmatrix = LRR.GetSalesMatrix("btd",0,0,"");
           // objviewmodel.lstsalesMatrices = lstsalesmatrix;
            return View(objviewmodel);
           // return PartialView("_SalesMatrixListing", lstsalesmatrix);
        }
        public ActionResult GetSalesMatrix(string searchData)
        {
            SalesMatrix objsales = new SalesMatrix();
            List<SalesMatrix> lstsalesmatrix = new List<SalesMatrix>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(searchData);
            foreach (Dictionary<string, object> item in objData)
            {
                var radiovalue = Convert.ToString(item["radio"]);
                var month = Convert.ToInt32(item["month"]);
                var sm = Convert.ToString(item["SalesManager"]);
                var year = Convert.ToInt32(item["year"]);
                lstsalesmatrix = LRR.GetSalesMatrix(radiovalue,month, year, sm);
              //  lstsalesmatrix.Add(objsales);
            }
            return PartialView("_SalesMatrixListing", lstsalesmatrix);
        }
        public ActionResult GetSalesMatrixDetails(string searchData)
        {
            //SalesMatrix objsalesmatrix = new SalesMatrix();
            //List<SalesMatrix> lstsalesmatrix = new List<SalesMatrix>();
            SalesMatrixDetail objsalesdetails = new SalesMatrixDetail();
            List<SalesMatrixDetail> lstsalesmatrixdetails = new List<SalesMatrixDetail>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(searchData);
            foreach (Dictionary<string, object> item in objData)
            {
                var radiovalue = Convert.ToString(item["radio"]);
                var month = Convert.ToInt32(item["month"]);
                var sm = Convert.ToString(item["SalesManager"]);
                var year = Convert.ToInt32(item["year"]);
                var type = Convert.ToString(item["type"]);
                lstsalesmatrixdetails = LRR.GetSalesMatrixDetails(radiovalue, month, year, sm,type);
                

            }
            return PartialView("_SalesMatrixDetails", lstsalesmatrixdetails);
        }
        public ActionResult PartnerReport()
        {
            return View(); 
        }

        public ActionResult GetPartnerReportList(string searchData)
        {
            List<PartnerReport> objPartnerReport = new List<PartnerReport>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(searchData);
            foreach (Dictionary<string, object> item in objData)
            {
                var isMTD = Convert.ToString(item["isMTD"]);
                var frmDate = Convert.ToString(item["frmDate"]);
                var toDate = Convert.ToString(item["toDate"]);

                objPartnerReport = LRR.GetPartnerReportData(frmDate, toDate, isMTD);
            }

            return PartialView("_PartnerReportListing", objPartnerReport);
        }
    }
}