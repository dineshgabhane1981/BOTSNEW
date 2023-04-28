

using BOTS_BL.Repository;
using OTPPage.Models;
using OTPPage.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OTPPage.Controllers
{
    public class OTPLoginController : Controller
    {
        OTPRepository OPR = new OTPRepository();
        Exception newexception = new Exception();
        //Class2 objdata = new Class2();

        // GET: OTPLogin
        public ActionResult Index()
        {
            return View();
            //return RedirectToAction("OTPPage");
    
        }

        //public ActionResult Authenticate(Class2 obj)
        //{
        //    objdata.LoginId = "123";
        //    objdata.Password = "123";

        //    if (objdata.LoginId == obj.LoginId && objdata.Password == obj.Password)
        //    {
        //        return RedirectToAction("OTPPage");
        //    }
        //    else
        //        return RedirectToAction("Index");
        //}

        public ActionResult OTPPage()
        {
            var groupId = "1051";
            OTPViewModel objdata = new OTPViewModel();
            objdata.lstGroupDetails = OPR.GetGroupDetails();
            return View(objdata);
        }

        [HttpPost]
        public ActionResult GetOTP(string jsonData)
        {
            OTPResponse obj = new OTPResponse();
            DataTable dt = new DataTable();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string GroupId = Convert.ToString(item["Groupid"]);
                string Mobileno = Convert.ToString(item["Mobileno"]);

                dt = OPR.GetOTPDetails(GroupId, Mobileno);
            }
            if (dt.Rows.Count>=1)
            {
                obj.OutletName = Convert.ToString(dt.Rows[0]["OutletName"]);
                obj.Datetime = Convert.ToString(dt.Rows[0]["Datetime"]);
                obj.OTP = Convert.ToString(dt.Rows[0]["OTP"]);
                obj.Points = Convert.ToString(dt.Rows[0]["PointsAvail"]);
            }
                
            //var Response = OPR.GetOTPDetails(GroupId,Mobileno);
            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}