using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using BOTS_BL.Models;
using System.Web.Script.Serialization;

namespace NPC.Controllers
{
    public class Login1Controller : Controller
    {
        NPCRepository NPCR = new NPCRepository();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveNPC(string jsonData)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            tblNPCDetail objOutletData = new tblNPCDetail();
            foreach (Dictionary<string, object> item in objData)
            {
                
                objOutletData.CustomerMobileNo = Convert.ToString(item["CustomerMobileNo"]);
                objOutletData.CustomerName = Convert.ToString(item["CustomerName"]);
                objOutletData.EmployeeName = Convert.ToString(item["EmployeeName"]);
                objOutletData.CategoryName = Convert.ToString(item["CategoryName"]);
                objOutletData.SubCategoryName = Convert.ToString(item["SubCategoryName"]);
                objOutletData.NextVisitDay = Convert.ToDecimal(item["NextVisitDay"]);
                objOutletData.Remarks = Convert.ToString(item["Remarks"]);
                
            }
            result = NPCR.SaveNPCData(objOutletData);

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }

    }
}