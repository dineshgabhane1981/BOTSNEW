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
            MeetingMatrix objMeetingMatrix = new MeetingMatrix();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(searchData);
            foreach (Dictionary<string, object> item in objData)
            {
                var salesManager = Convert.ToString(item["SalesManager"]);
                var frmDate = Convert.ToString(item["frmDate"]);
                var toDate = Convert.ToString(item["toDate"]);
                var MeetingOrCall = Convert.ToString(item["MeetingOrCall"]);
                objMeetingMatrix = LRR.GetMeetingMatrixReport(salesManager, frmDate, toDate, MeetingOrCall);
            }
                
            return PartialView("_MeetingMatrixListing", objMeetingMatrix);
        }
        public ActionResult SalesMatrix()
        {
            return View();
        }
        public ActionResult CallingMatrix()
        {
            return View();
        }
    }
}