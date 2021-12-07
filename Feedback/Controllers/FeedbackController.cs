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
    public class FeedbackController : Controller
    {
        Exceptions newexception = new Exceptions();
        FeedBackRepository FBR = new FeedBackRepository();// GET: Feedback
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

        public ActionResult GetCustomerStatus(string mobileNo, string GroupId)
        {           
            CustomerDetailwithFeedback obj = new CustomerDetailwithFeedback();
            try
            {
                obj = FBR.GetCustomerInfo(mobileNo, GroupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Get feedbackcust");
            }
            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public ActionResult SubmitRating(string mobileNo, int[] ranking, string GroupId, string outletId)
        {
            string status = "false";            
            CustomerDetail objcustomerdetails = new CustomerDetail();
            try
            {
                status = FBR.SubmitRating(mobileNo, ranking, GroupId, outletId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "submit rating");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }

        public ActionResult SubmitPoints(string MemberName, string Gender,string BirthDt, string mobileNo, string AnniversaryDt, string LiveIn, string Knowabt, string GroupId, string OutletId)
        {
            bool status = false;
            CustomerDetail objcustomerdetails = new CustomerDetail();
            try
            {
                status = FBR.SubmitPoints(MemberName, Gender, BirthDt, mobileNo, AnniversaryDt, LiveIn, Knowabt, GroupId, OutletId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "submit points");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
    }
}